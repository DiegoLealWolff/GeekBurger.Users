using GeekBurger.Users.Model;
using GeekBurger.Users.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Services
{
    public class FacialServices : IFacialServices
    {
        public static IConfiguration _Configuration;
        private IUsersRepository _usersRepository;
        public static FaceServiceClient _faceServiceClient;
        public static Guid faceListId;

        public FacialServices(IUsersRepository usersRepository)
        {
            _Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _usersRepository = usersRepository;

            _faceServiceClient = new FaceServiceClient(_Configuration["FaceAPIKey"], _Configuration["FaceEndPoint"]);            
        }   

        public UserModel StartValidade(string faceInput)
        {
            var resultUsersRepository = _usersRepository.GetUsers();
            var templateImage = @"Faces\{0}.jpg";
            var sourceImage = "";

            faceListId = Guid.Parse(_Configuration["FaceListId"]);

            try
            {
                if (resultUsersRepository.Exists(x => x.Face == faceInput))
                {
                    return resultUsersRepository.Where(x => x.Face == faceInput).FirstOrDefault();
                }
                else
                {
                    sourceImage = string.Format(templateImage, faceInput);   
                    
                    var containsAnyFaceOnList = UpsertFaceListAndCheckIfContainsFaceAsync().Result;

                    var face = DetectFaceAsync(sourceImage).Result;

                    if (face != null)
                    {

                        Guid? persistedId = null;

                        if (containsAnyFaceOnList)
                        {
                            persistedId = FindSimilarAsync(face.FaceId, faceInput).Result;
                        }

                        if (persistedId == null)
                        {
                            var guid = new Guid(faceInput);
                            persistedId = AddFaceAsync(guid, sourceImage).Result;                            
                        }

                        var modelUser = new UserModel()
                        {
                            Face = faceInput,
                            UserId = Convert.ToInt32(persistedId.Value),
                            AreRestrictionsSet = "false"
                        };

                        _usersRepository.Add(modelUser);

                        return modelUser;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpsertFaceListAndCheckIfContainsFaceAsync()
        {
            try
            {
                var faceListIdIn = faceListId.ToString();
                var faceLists = await _faceServiceClient.ListFaceListsAsync();
                var faceList = faceLists.FirstOrDefault(_ => _.FaceListId == faceListIdIn);

                if (faceList == null)
                {
                    await _faceServiceClient.CreateFaceListAsync(faceListIdIn, "GeekBurgerFaces", null);
                    return false;
                }

                var faceListJustCreated = await _faceServiceClient.GetFaceListAsync(faceListIdIn);

                return faceListJustCreated.PersistedFaces.Any();
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<Guid?> FindSimilarAsync(Guid faceId, string faceListId)
        {
            try
            {
                var similarFaces = await _faceServiceClient.FindSimilarAsync(faceId, faceListId.ToString());

                var similarFace = similarFaces.FirstOrDefault(_ => _.Confidence > 0.5);

                return similarFace?.PersistedFaceId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Face> DetectFaceAsync(string imageFilePath)
        {
            try
            {
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    var faces = await _faceServiceClient.DetectAsync(imageFileStream);
                    
                    return faces.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Guid?> AddFaceAsync(Guid faceListId, string imageFilePath)
        {
            try
            {
                AddPersistedFaceResult faceResult;
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    faceResult = await _faceServiceClient.AddFaceToFaceListAsync(faceListId.ToString(), imageFileStream);
                    return faceResult.PersistedFaceId;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
