using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.ImageRender
{
    public class ResultC
    {
        public int NumBits { get; set; }
        public string Text { get; set; }
        public IDictionary<ResultMetadataType,object> ResultMetadata { get; set; }
    }
    public enum ResultMetadataType
    {
        OTHER = 0,
        ORIENTATION = 1,
        BYTE_SEGMENTS = 2,
        ERROR_CORRECTION_LEVEL = 3,
        ISSUE_NUMBER = 4,
        SUGGESTED_PRICE = 5,
        POSSIBLE_COUNTRY = 6,
        UPC_EAN_EXTENSION = 7,
        STRUCTURED_APPEND_SEQUENCE = 8,
        STRUCTURED_APPEND_PARITY = 9,
        PDF417_EXTRA_METADATA = 10,
        AZTEC_EXTRA_METADATA = 11,
        SYMBOLOGY_IDENTIFIER = 12
    }
}
