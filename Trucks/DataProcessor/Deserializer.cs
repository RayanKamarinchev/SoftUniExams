using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Trucks.Data.Models;
using Trucks.Data.Models.Enums;
using Trucks.DataProcessor.ImportDto;

namespace Trucks.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportDespacherDto[]), new XmlRootAttribute("Despatchers"));

            List<Despatcher> despachers = new List<Despatcher>();

            using (StringReader stringReader = new StringReader(xmlString))
            {
                ImportDespacherDto[] despacherDtos = (ImportDespacherDto[])xmlSerializer.Deserialize(stringReader);
                foreach (var despacherDto in despacherDtos)
                {
                    if (!IsValid(despacherDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    List<Truck> trucks = new List<Truck>();
                    foreach (var truckDto in despacherDto.Trucks)
                    {
                        if (!IsValid(truckDto))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        MakeType make = (MakeType)truckDto.MakeType;
                        if (!Enum.IsDefined(typeof(MakeType), make) && !make.ToString().Contains(","))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                        CategoryType category = (CategoryType)truckDto.CategoryType;
                        if (!Enum.IsDefined(typeof(CategoryType), category) && !category.ToString().Contains(","))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                        trucks.Add(new Truck()
                        {
                            RegistrationNumber = truckDto.RegistrationNumber,
                            VinNumber = truckDto.VinNumber,
                            TankCapacity = truckDto.TankCapacity,
                            CargoCapacity = truckDto.CargoCapacity,
                            CategoryType = category,
                            MakeType = make
                        });
                    }
                    despachers.Add(new Despatcher()
                    {
                        Name = despacherDto.Name,
                        Position = despacherDto.Position,
                        Trucks = trucks
                    });
                    sb.AppendLine($"Successfully imported despatcher - {despacherDto.Name} with {trucks.Count} trucks.");
                }
                context.Despatchers
                       .AddRange(despachers);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportClientDto[] clientsDtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);
            List<Client> clients = new List<Client>();
            foreach (var clientDto in clientsDtos)
            {
                if (!IsValid(clientDto)||clientDto.Type=="usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<ClientTruck> trucks = new List<ClientTruck>();
                List<int> triedIds = new List<int>();
                foreach (var truckId in clientDto.Trucks)
                {
                    if (triedIds.Contains(truckId))
                    {
                        triedIds.Add(truckId);
                        continue;
                    }
                    if (context.Trucks.Find(truckId)==null)
                    {
                        triedIds.Add(truckId);
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    triedIds.Add(truckId);
                    trucks.Add(new ClientTruck()
                    {
                        TruckId = truckId
                    });
                }

                clients.Add(new Client()
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type,
                    ClientsTrucks = trucks
                });
                sb.AppendLine(
                    $"Successfully imported client - {clientDto.Name} with {trucks.Count} trucks.");
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
