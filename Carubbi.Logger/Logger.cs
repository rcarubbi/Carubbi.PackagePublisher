using System;
using System.IO;
using System.Text;
namespace Carubbi.Logger
{
    /// <summary>
    /// Gerador de Log
    /// </summary>
    public abstract class Logger
    {
        protected LogLevel logMask;

        // The next Handler in the chain
        protected Logger _next;
        protected Func<String, String> _formatMessageHandler;


        /// <summary>
        /// Ao definir um nível de log para uma instância, está só irá gravar o log caso a mensagem enviada atinja o nível definido
        /// </summary>
        /// <param name="mask"></param>
        public Logger(LogLevel mask)
            : this(mask, (str) => string.Format("{0} - {1}", DateTime.Now, str))
        { }


        /// <summary>
        /// Constroi um logger informado nível de criticidade a ser gravado e padrão de transformação do texto
        /// </summary>
        /// <param name="severity">Nível de criticidade</param>
        /// <param name="formatMessageHandler">Método que transforma o texto a ser logado para receber um padrão de exibição</param>
        public Logger(LogLevel mask, Func<String, String> formatMessageHandler)
        {
            this.logMask = mask;
            _formatMessageHandler = formatMessageHandler;
        }

        /// <summary>
        /// Define o próximo Logger para montar uma cadeia de responsabilidades, os Loggers são definidos um dentro do outro e vão se acumulando cada um com sua estratégia de gravação do log e um nível de criticidade definido
        /// </summary>
        public Logger SetNext(Logger nextlogger)
        {
            _next = nextlogger;
            return nextlogger;
        }


        /// <summary>
        /// Escreve uma mensagem de exceção atribuindo um determinado nível de criticidade a ela
        /// </summary>
        /// <param name="msg">Exceção a ser logada</param>
        /// <param name="severity">Nível de criticidade</param>
        public void Message(Exception msg, LogLevel severity)
        {
            StringBuilder stbExceptions = new StringBuilder();
            stbExceptions.AppendFormat("Message: {0}, Stacktrace: {1}, Source: {2}", msg.Message, msg.StackTrace, msg.Source);
            var innerException = msg.InnerException;
            while (innerException != null)
            {
                stbExceptions.AppendFormat("Message: {0}, Stacktrace: {1}, Source: {2}", innerException.Message, innerException.StackTrace, innerException.Source);
                innerException = innerException.InnerException;
            }
            Message(stbExceptions.ToString(), severity);
        }

        /// <summary>
        /// Escreve uma mensagem de texto atribuindo um determinado nível de criticidade a ela
        /// </summary>
        /// <param name="msg">Mensagem a ser logada</param>
        /// <param name="severity">Nível de criticidade</param>
        public void Message(string msg, LogLevel severity)
        {
            if ((severity & logMask) != 0) //True only if all logMask bits are set in severity
            {
                WriteMessage(msg);
            }
            if (_next != null)
            {
                _next.Message(_formatMessageHandler(msg), severity);
            }
        }

        /// <summary>
        /// Deve ser implementado com a estratégia de gravação de log da classe concreta
        /// </summary>
        /// <param name="msg"></param>
        abstract protected void WriteMessage(string msg);

        /// <summary>
        /// Retrocompatibilidade para sistemas que utilizam versão legado do Logger
        /// </summary>
        /// <param name="exception"></param>
        [Obsolete]
        public static void LogException(Exception exception)
        {
            var logger = new FileLogger();
            logger.Message(exception, LogLevel.Error);
            logger = null;
        }
    }
}


