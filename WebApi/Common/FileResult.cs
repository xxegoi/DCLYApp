using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApi.Common
{
    /// <summary>
    /// 实现一个HttpResponseMessage异步命令，用于文件传输
    /// </summary>
    public class FileResult : IHttpActionResult
    {
        //private readonly string _filePath;
        private readonly Stream _fileStream;
        private readonly string _fileName;
        private readonly string _contentType;


        //public FileResult(string filePath, string contentType = null)
        //{
        //    if (filePath == null) throw new ArgumentNullException("filePath");

        //    _filePath = filePath;
        //    _contentType = contentType;
        //}

        /// <summary>
        /// 传入文件或者流的二进制数组以及文件名，文件名用于映射MIME类型，contentType可以默认为空
        /// </summary>
        public FileResult(byte[] data, string fileName, string contentType = null)
        {
            if (data == null) throw new ArgumentNullException("_fileStream");

            _fileStream = new MemoryStream(data);
            _fileName = fileName;
            _contentType = contentType;
        }

        /// <summary>
        /// IHttpActionResult的ExecuteAsync方法实现
        /// </summary>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                //Content = new StreamContent(File.OpenRead(_filePath))
                Content = new StreamContent(_fileStream)
            };

            var contentType = _contentType ?? MimeMapping.GetMimeMapping(Path.GetExtension(_fileName));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return Task.FromResult(response);
        }
    }
}