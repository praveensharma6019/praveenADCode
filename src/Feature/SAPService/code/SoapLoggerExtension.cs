using System;
using System.IO;
using System.Web.Services.Protocols;
using NLog;

namespace SapPiService
{
    public class SoapLoggerExtension : SoapExtension
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Stream _oldStream;
        private Stream _newStream;

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {

        }

        public override Stream ChainStream(Stream stream)
        {
            _oldStream = stream;
            _newStream = new MemoryStream();
            return _newStream;
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    Log(message, "AfterSerialize");
                    CopyStream(_newStream, _oldStream);
                    _newStream.Position = 0;
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    CopyStream(_oldStream, _newStream);
                    Log(message, "BeforeDeserialize");
                    break;
                case SoapMessageStage.AfterDeserialize:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Log(SoapMessage message, string stage)
        {
            _newStream.Position = 0;
            var contents = (message is SoapServerMessage) ? "SoapRequest " : "SoapResponse ";
            contents += stage + ";";

            var reader = new StreamReader(_newStream);

            contents += reader.ReadToEnd();

            _newStream.Position = 0;

            Logger.Debug(contents);
        }

        private static void CopyStream(Stream fromStream, Stream toStream)
        {
            try
            {
                var sr = new StreamReader(fromStream);
                var sw = new StreamWriter(toStream);
                sw.WriteLine(sr.ReadToEnd());
                sw.Flush();
            }
            catch (Exception ex)
            {
                var message = $"CopyStream failed because: {ex.Message}";
                Logger.Error(ex, message);
            }
        }
    }
}