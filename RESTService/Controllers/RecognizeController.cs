﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoreParallelRecognition;

namespace RESTService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecognizeController : ControllerBase
    {
        [HttpPost]
        public ImageClassified RecognizeImage([FromBody] FileDescription fileDescription)
        {
            var contents = Convert.FromBase64String(fileDescription.Content);
            return ParallelRecognition.RecognizeContents(contents, fileDescription.Name);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            using var db = new RecognitionModelContainer();
            return (from row in db.Results select row.Filename + "|" + row.HitCount.ToString()).ToArray();
        }

        [HttpDelete]
        public void TruncateStats()
        {
            using var db = new RecognitionModelContainer();
            db.Database.ExecuteSqlCommand("DELETE Results");
            db.Database.ExecuteSqlCommand("DELETE Blobs");
        }
    }

    public class FileDescription
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
