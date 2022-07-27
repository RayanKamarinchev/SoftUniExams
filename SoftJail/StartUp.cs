namespace SoftJail
{
    using System;
    using Data;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.IO;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new SoftJailDbContext();

            Mapper.Initialize(config => config.AddProfile<SoftJailProfile>());

            ResetDatabase(context, shouldDropDatabase: false);

            var projectDir = GetProjectDirectory();

            //ImportEntities(context, projectDir + @"Datasets/", projectDir + @"ImportResults/");
            ExportEntities(context, projectDir + @"ExportResults/");

            //using (var transaction = context.Database.BeginTransaction())
            //{
            //    transaction.Rollback();
            //}
        }

        private static void ImportEntities(SoftJailDbContext context, string baseDir, string exportDir)
        {
            var departmentsCells =
                DataProcessor.Deserializer.ImportDepartmentsCells(context,
                    File.ReadAllText(baseDir + "ImportDepartmentsCells.json"));
            PrintAndExportEntityToFile(departmentsCells, exportDir + "ImportDepartmentsCells.txt");

            var prisonersMails =
                DataProcessor.Deserializer.ImportPrisonersMails(
                    context,
                    "[{'FullName':'Rosmunda Yoodall','Nickname':'The Lappet','Age':46,'IncarcerationDate':'18/05/1965','ReleaseDate':'19/06/2006','Bail':86810.94,'CellId':17,'Mails':[{'Description':'So here is the code. This will make it really easy to update our data.','Sender':'Billye Hakey','Address':'64 Sugar Plaza str.'},{'Description':'You know… (techno) Like The Eagles!','Sender':'Tanya Ligertwood','Address':'290 Jenna Court str.'},{'Description':'What if I find his head from another photo pointing in the right direction?','Sender':'El Done','Address':'3887 Luster Drive str.'}]},{'FullName':'Benji Ballefant','Nickname':'The Peccary','Age':38,'IncarcerationDate':'12/09/1967','ReleaseDate':'07/02/1989','Bail':93934.2,'CellId':4,'Mails':[{'Description':'Okay, I have finished my data entry for June.','Sender':'Leona Cutford','Address':'43901 Dwight Trail str.'},{'Description':'That is fine, take your time no rush. I like your work and we will like you to take your time.','Sender':'Augustine Eickhoff','Address':'6 Riverside Trail str.'}]},{'FullName':'Aguistin Rawls','Nickname':'The Sunbird','Age':25,'IncarcerationDate':'30/08/1955','ReleaseDate':'29/09/2005','Bail':90533.66,'CellId':12,'Mails':[{'Description':'I do a lot of work for local bands.','Sender':'Dynah Lawee','Address':'751 Linden Hill str.'}]}]"); //File.ReadAllText(baseDir + "ImportPrisonersMails.json"));
            PrintAndExportEntityToFile(prisonersMails, exportDir + "ImportPrisonersMails.txt");

            var officersPrisoners = DataProcessor.Deserializer.ImportOfficersPrisoners(context, File.ReadAllText(baseDir + "ImportOfficersPrisoners.xml"));
            PrintAndExportEntityToFile(officersPrisoners, exportDir + "ImportOfficersPrisoners.txt");
        }

        private static void ExportEntities(SoftJailDbContext context, string exportDir)
        {
            //var jsonOutput = DataProcessor.Serializer.ExportPrisonersByCells(context, new[] { 1, 5, 7, 3 });
            //Console.WriteLine(jsonOutput);
            //File.WriteAllText(exportDir + "PrisonersByCells.json", jsonOutput);

            var xmlOutput = DataProcessor.Serializer.ExportPrisonersInbox(context, "Melanie Simonich,Diana Ebbs,Binni Cornhill");
            Console.WriteLine(xmlOutput);
            File.WriteAllText(exportDir + "PrisonersInbox.xml", xmlOutput);
        }
        private static void ResetDatabase(SoftJailDbContext context, bool shouldDropDatabase = false)
        {
            if (shouldDropDatabase)
            {
                context.Database.EnsureDeleted();
            }

            if (context.Database.EnsureCreated())
            {
                return;
            }

            var disableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(disableIntegrityChecksQuery);

            var deleteRowsQuery = "EXEC sp_MSforeachtable @command1='SET QUOTED_IDENTIFIER ON;DELETE FROM ?'";
            context.Database.ExecuteSqlCommand(deleteRowsQuery);

            var enableIntegrityChecksQuery =
                "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(enableIntegrityChecksQuery);

            var reseedQuery =
                "EXEC sp_MSforeachtable @command1='IF OBJECT_ID(''?'') IN (SELECT OBJECT_ID FROM SYS.IDENTITY_COLUMNS) DBCC CHECKIDENT(''?'', RESEED, 0)'";
            context.Database.ExecuteSqlCommand(reseedQuery);
        }

        private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
        {
            Console.WriteLine(entityOutput);
            File.WriteAllText(outputPath, entityOutput.TrimEnd());
        }

        private static string GetProjectDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryName = Path.GetFileName(currentDirectory);
            var relativePath = directoryName.StartsWith("netcoreapp") ? @"../../../" : string.Empty;

            return relativePath;
        }
    }
}