using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Theatre.Data.Models;
using Theatre.Data.Models.Enums;
using Theatre.DataProcessor.ImportDto;

namespace Theatre.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Theatre.Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer =
                new XmlSerializer
                (typeof(ImportPlayDto[])
                 , new XmlRootAttribute("Plays"));

            List<Play> plays = new List<Play>();

            using (StringReader stringReader = new StringReader(xmlString))
            {
                ImportPlayDto[] playsDtos =
                    (ImportPlayDto[])xmlSerializer
                        .Deserialize(stringReader);
                foreach (var playDto in playsDtos)
                {
                    if (!IsValid(playDto))
                    {
                        Console.WriteLine(ErrorMessage);
                        continue;
                    }

                    TimeSpan duration;
                    bool canParse =
                        TimeSpan
                            .TryParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture, out duration);
                    if (!canParse || duration.Hours > 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Genre genre;
                    bool canGenre = Enum.TryParse(playDto.Genre, out genre);
                    if (!canGenre)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    plays.Add(new Play()
                    {
                        Description = playDto.Description,
                        Genre = genre,
                        Duration = duration,
                        Rating = playDto.Rating,
                        Screenwriter = playDto.Screenwriter,
                        Title = playDto.Title
                    });
                    sb.AppendLine(
                        $"Successfully imported {playDto.Title} with genre {genre.ToString()} and a rating of {playDto.Rating}!");
                }
                context.AddRange(plays);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            throw new NotImplementedException();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}