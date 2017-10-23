using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;



namespace TrustedAuthorityServer
{
    //These are the interface declerations for the client
    [ServiceContract]
    interface IMessageCallback
    {
        //This is the callback interface decleration for the client
        [OperationContract(IsOneWay = true)]
        void OnMessageAdded(string message, DateTime timestamp);
    }


    [ServiceContract(CallbackContract = typeof(IMessageCallback))]
    public interface IMessage
    {
        //these are the interface decleratons for the server.
        [OperationContract]
        void AddMessage(string message);
        [OperationContract]
        bool Subscribe();
        [OperationContract]
        bool Unsubscribe();
    }



    class RCRProxy : IMessageCallback, IDisposable
    {
        IMessage pipeProxy = null;
        public bool Connect()
        {
            //note the "DuplexChannelFactory".  This is necessary for Callbacks.
            // A regular "ChannelFactory" won't work with callbacks.
            DuplexChannelFactory<IMessage> pipeFactory =
                  new DuplexChannelFactory<IMessage>(
                      new InstanceContext(this),
                      new NetTcpBinding(),
                      new EndpointAddress("net.tcp://localhost:8000/ISubscribe"));


            try
            {
                //Open the channel to the server
                pipeProxy = pipeFactory.CreateChannel();
                //Now tell the server who is connecting
                pipeProxy.Subscribe();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public void Close()
        {
            pipeProxy.Unsubscribe();
        }



        //This function sends a string to the server so that it can broadcast
        // it to all other clients that have called Subscribe().
        public string SendMessage(string message)
        {
            try
            {
                pipeProxy.AddMessage(message);
                return "sent >>>>  " + message;
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        //This is the function that the SERVER will call
        public void OnMessageAdded(string message, DateTime timestamp)
        {
            Console.WriteLine(message + ": " + timestamp.ToString("hh:mm:ss"));
        }

        //We need to tell the server that we are leaving
        public void Dispose()
        {
            pipeProxy.Unsubscribe();
        }


    }
}
