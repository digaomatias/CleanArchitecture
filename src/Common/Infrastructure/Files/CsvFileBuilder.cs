using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Application.Common.Interfaces;
using Application.Dto;
using CsvHelper;
using Infrastructure.Files.Maps;
using Mapster;

namespace Infrastructure.Files
{
    public class CsvFileBuilder : ICsvFileBuilder
    {
        public byte[] BuildDistrictsFile(IEnumerable<DistrictDto> cities)
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
                
                csvWriter.WriteRecords(cities);
            }

            return memoryStream.ToArray();
        }
    }
}
