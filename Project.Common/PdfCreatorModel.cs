using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common
{
    public class PdfCreatorModel
    {
        public Guid Id { get; set; }
        public string PdfUrl { get; set; }
        public DateTime ResponseTime { get; set; }
        public TranscriptModel TranscriptModel { get; set; }

        public PdfCreatorModel()
        {
            Id = Guid.NewGuid();
            ResponseTime = DateTime.Now;
        }
    }
}
