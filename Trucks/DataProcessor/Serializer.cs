using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Trucks.Data.Models;
using Trucks.Data.Models.Enums;
using Trucks.DataProcessor.ExportDto;

namespace Trucks.DataProcessor
{
    using System;

    using Data;

    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportDespatcher[]), new XmlRootAttribute("Despatchers"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringBuilder sb = new StringBuilder();

            List<ExportDespatcher> despatchersDtos = new List<ExportDespatcher>();
            List<Despatcher> despatchers = despatchers
                = context.Despatchers
                         .Where(d => d.Trucks.Count > 0)
                         .OrderByDescending(d => d.Trucks.Count)
                         .ThenBy(d => d.Name)
                         .ToList();
            foreach (var despatcher in despatchers)
            {
                List<ExportDespatcherTruck> trucks = new List<ExportDespatcherTruck>();
                foreach (var truck in despatcher.Trucks)
                {
                    trucks.Add(new ExportDespatcherTruck()
                    {
                        RegistrationNumber = truck.RegistrationNumber,
                        Make = truck.MakeType.ToString()
                    });
                }
                despatchersDtos.Add(new ExportDespatcher()
                {
                    Trucks = trucks.OrderBy(t => t.RegistrationNumber).ToArray(),
                    TrucksCount = trucks.Count(),
                    DespatcherName = despatcher.Name
                });
            }

            using (StringWriter stringWriter = new StringWriter(sb))
            {
                xmlSerializer.Serialize(stringWriter, despatchersDtos.ToArray(), namespaces);
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            StringBuilder sb = new StringBuilder();
            List<ExportClient> clientsDtos = new List<ExportClient>();
            List<Client> clients = context.Clients
                                          .Where(c => c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                                          .OrderByDescending(c=>c.ClientsTrucks.Where(ct=>ct.Truck.TankCapacity>=capacity).Count())
                                          .ThenBy(c=>c.Name)
                                          .Take(10)
                                          .ToList();
            foreach (var client in clients)
            {
                List<ExportTruck> trucksDtos = new List<ExportTruck>();
                foreach (var truck in client.ClientsTrucks.Where(ct => ct.Truck.TankCapacity >= capacity))
                {
                    Truck t = truck.Truck;
                    trucksDtos.Add(new ExportTruck()
                    {
                        CargoCapacity = t.CargoCapacity,
                        CategoryType = t.CategoryType.ToString(),
                        MakeType = t.MakeType.ToString(),
                        TruckRegistrationNumber = t.RegistrationNumber,
                        TankCapacity = t.TankCapacity,
                        VinNumber = t.VinNumber
                    });
                }
                clientsDtos.Add(new ExportClient()
                {
                    Name = client.Name,
                    Trucks = trucksDtos.OrderBy(t=>t.MakeType)
                                       .ThenByDescending(t=>t.CargoCapacity)
                                       .ToArray()
                });
            }
            return JsonConvert.SerializeObject(clientsDtos);
        }
    }
}
