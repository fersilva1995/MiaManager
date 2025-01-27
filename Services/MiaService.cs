using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using MiaGRPC;
using MiaManager.Models;
using MiaManager.Services;
using MiaManager.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace MiaManager
{
    public class MiaService
    {

        #region SINGLETON

        private MiaService()
        {

        }

        private static MiaService? instance = null;
        public static MiaService Instance
        {
            get
            {
                instance ??= new();
                return instance;
            }
        }

        #endregion


        public string address = "http://127.0.0.1:50051";

        public GrpcChannel GetChannel()
        {
            var channel = GrpcChannel.ForAddress(new Uri(address), new GrpcChannelOptions
            {
                MaxSendMessageSize = 100 * 1024 * 1024, // 100 MiB
                MaxReceiveMessageSize = 100 * 1024 * 1024
            });
            return channel;
        }

        public MiaGRPC.MiaService.MiaServiceClient GetClient()
        {

            var client = new MiaGRPC.MiaService.MiaServiceClient(GetChannel());
            return client;
        }

        #region USER

        public async Task<string> GetUsers()
        {
            try
            {
                var reply = await GetClient().ReadUsersAsync(new MiaGRPC.SetUserRequest { Name = "" });
                return reply.Data.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }


        public async Task<string> AddUser(User user)
        {
            try
            {
                var reply = await GetClient().CreateUserAsync(new MiaGRPC.SetUserRequest { Id = "", Name = user.Name });
                return reply.Data.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }


        public async Task<string> PutUser(User user)
        {
            try
            {
                var reply = await GetClient().UpdateUserAsync(new MiaGRPC.SetUserRequest { Id = user.Id, Name = user.Name });
                return reply.Data.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }



        public async Task<string> RemoveUser(User user)
        {
            try
            {
                var reply = await GetClient().DeleteUserAsync(new MiaGRPC.SetUserRequest { Id = user.Id, Name = user.Name });
                return reply.Data.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }

        #endregion


        public async Task<string> GetSvms()
        {
            try
            {
                var reply = await GetClient().ReadSvmsAsync(new MiaGRPC.SetSvmRequest());
                return reply.Data.ToString();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }


        public async Task<string> AddSvm(Svm svm)
        {
            try
            {
                SetSvmRequest request = new()
                {
                    Id = svm.Id,
                    Name = svm.Name,
                    CreateNegative = svm.CreateNegative,
                    CreateUnknown = svm.CreateUnknown,
                };
                foreach (string id in svm.Users)
                    request.Users.Add(id);

                var reply = await GetClient().CreateSvmAsync(request);
                return reply.Data.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }


        public async Task<string> PutSvm(Svm svm)
        {
            try
            {
                SetSvmRequest request = new() { 
                    Id = svm.Id, 
                    Name = svm.Name, 
                    CreateNegative = svm.CreateNegative , 
                    CreateUnknown = svm.CreateUnknown, 
                };
                foreach (string id in svm.Users)
                    request.Users.Add(id);

                var reply = await GetClient().UpdateSvmAsync(request);
                return reply.Data.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }



        public async Task<string> RemoveSvm(Svm svm)
        {
            try
            {
                var reply = await GetClient().DeleteSvmAsync(new MiaGRPC.SetSvmRequest { Id = svm.Id });
                return reply.Data.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }

        public async Task<string> TrainSvm(Svm svm)
        {
            try
            {
                var reply = await GetClient().TrainSvmAsync(new MiaGRPC.SetSvmRequest { Id = svm.Id });
                return reply.Response.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }


        #region Image

        public async Task<List<ImageData>> ReadIndex(string id, string source)
        {
            try
            {
                var reply = await GetClient().ReadIndexAsync(new IndexRequest { Source = source, UserId = id });
                return [.. reply.Data];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new();
            }
        }


        public async Task<List<object>> ReadData(string id, string source, List<string> files)
        {

            List<object> data = [];
            var channel = GrpcChannel.ForAddress(new Uri(address), new GrpcChannelOptions
            {
                MaxSendMessageSize = 100 * 1024 * 1024, // 100 MiB
                MaxReceiveMessageSize = 100 * 1024 * 1024
            });


            var client = new MiaGRPC.MiaService.MiaServiceClient(channel);

            var request = new DataRequest() { UserId = id, Source = source };
            foreach (string file in files)
                request.Ids.Add(file);

            using var call = client.ReadData(request);
            var cts = new CancellationTokenSource();

            ProgressService.Instance.Stop = false;
            ProgressService.Instance.Max = files.Count;
            ProgressService.Instance.Steps = files.Count;
            ProgressView progressView = new();
            progressView.Show();


            try
            {
                while (await call.ResponseStream.MoveNext(cts.Token) && !ProgressService.Instance.Stop)
                {
                    var response = call.ResponseStream.Current;
                    data.Add(response);
                    ProgressService.Instance.Step(response.Id + "...");
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Progress monitoring was canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while monitoring progress: {ex.Message}");
            }

            return data;
        }

        public async Task<bool> SetData(List<Data> data, string source, string userId)
        {
            if (ProgressService.Instance.Stop)
                return true;

            ProgressService.Instance.Value = 0;
            ProgressService.Instance.Max = data.Count;
            ProgressService.Instance.Steps = data.Count;
            ProgressView progressView = new();
            progressView.Show();

            var call = GetClient().SetData();

            foreach (Data imageData in data)
            {
                if (!ProgressService.Instance.Stop)
                {
                    byte[] bytes = [.. imageData.Value];
                    var request = new SetDataRequest { Id = userId, Source = source, Name = imageData.Name, Reference = imageData.Id, ImageBytes = ByteString.CopyFrom(bytes) };
                    await call.RequestStream.WriteAsync(request);
                    await Task.Delay(500);

                    ProgressService.Instance.Step(imageData.Name + "...");
                }
                else
                    break;
          
            }

            await call.RequestStream.CompleteAsync();
            var response = await call.ResponseAsync;
            if (response.Response == "sucess")
                return true;

            return false;
        }

        public async Task<bool> RemoveData(string userId, string source, List<string> ids)
        {
            RemoveDataRequest removeDataRequest = new() { Source = source, UserId = userId };
            foreach (string id in ids)
                removeDataRequest.Ids.Add(id);
            var reply = await GetClient().RemoveDataAsync(removeDataRequest);
            if (reply.Response == "sucess")
                return true;

            return false;
        }

        public async Task<List<RecognitionResponse>> Recognize(List<Data> data)
        {
            try
            {
                List<RecognitionResponse> responses = [];
                var client = GetClient();

                var call = client.Recognize();
                bool completeSend = false;
                // Sending messages to the server
                var sendTask = Task.Run(async () =>
                {
                    foreach (var message in data)
                    {
                        byte[] bytes = [.. message.Value];
                        RecognitionRequest request = new() { Image = ByteString.CopyFrom(bytes) };
                        await call.RequestStream.WriteAsync(request);
                        await Task.Delay(500);
                    }

                    completeSend = true;    
                    await call.RequestStream.CompleteAsync();
                });

                // Receiving messages from the server
                var receiveTask = Task.Run(async () =>
                {
                    while(completeSend == false)
                    {
                        await foreach (var response in call.ResponseStream.ReadAllAsync())
                        {
                            responses.Add(response);
                        }

                        Thread.Sleep(1000);
                    }
          
                });

                await Task.WhenAll(sendTask, receiveTask);

                return responses;
            }
            catch (Exception e)
            {
                return new();
            }
        }

        #endregion
    }
}
