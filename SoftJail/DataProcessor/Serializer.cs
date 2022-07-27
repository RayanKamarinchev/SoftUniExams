using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftJail.DataProcessor.ExportDto;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            ExportPrisonerDto[] prisoners = 
            context.Prisoners
                   .Include(p=> p.PrisonerOfficers)
                   .ThenInclude(p=>p.Officer)
                   .Where(p => ids.Contains(p.Id))
                   .OrderBy(p => p.FullName)
                   .ThenBy(p => p.Id)
                   .Select(p => new ExportPrisonerDto()
                   {
                       Name = p.FullName,
                       Id = p.Id,
                       CellNumber = p.Cell.CellNumber,
                       Officers = p.PrisonerOfficers.Select(po => new ExportOfficer()
                       {
                           Department = po.Officer.Department.Name,
                           Name = po.Officer.FullName
                       })
                                   .OrderBy(o=>o.Name)
                                   .ToArray(),
                       TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Select(po => po.Officer).Sum(o => o.Salary), 2)
                   }).ToArray();
            return JsonConvert.SerializeObject(prisoners);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportPrisonerWithMessages[]), new XmlRootAttribute("Prisoners"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringBuilder sb= new StringBuilder();

            string[] names = prisonersNames.Split(',');
            ExportPrisonerWithMessages[] prisoners
                = context.Prisoners
                        .Where(p => names.Contains(p.FullName))
                        .OrderBy(p => p.FullName)
                        .ThenBy(p => p.Id)
                        .Select(p => new ExportPrisonerWithMessages()
                        {
                            Id = p.Id,
                            Name = p.FullName,
                            IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture),
                            EncryptedMessages = p.Mails.Select(m => new ExportMessage()
                            {
                                Description = Reverse(m.Description)
                            })
                                                 .ToArray()
                        })
                        .ToArray();

            using (StringWriter stringWriter = new StringWriter(sb))
            {
                xmlSerializer.Serialize(stringWriter, prisoners, namespaces);
            }
            return sb.ToString().TrimEnd();
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}