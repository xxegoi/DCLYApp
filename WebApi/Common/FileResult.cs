﻿using System;
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
    class FileResult : IHttpActionResult
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

        public FileResult(byte[] data, string fileName, string contentType = null)
        {
            if (data == null) throw new ArgumentNullException("_fileStream");

            _fileStream = new MemoryStream(data);
            _fileName = fileName;
            _contentType = contentType;
        }

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