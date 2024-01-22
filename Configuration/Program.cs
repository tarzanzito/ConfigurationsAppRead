
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Configuration.Xml;

namespace ConfigurationsAppRead
{
    public class Program
    {
        //Configuration Types (Providers) (Sources)
        //-----------------------------------------
        //REF 1-Settings files, such as appsettings.json, ini,  xml
        //REF 2-Environment variables
        //REF 3-Azure Key Vault
        //REF 4-Azure App Configuration
        //REF 5-Command - line arguments
        //REF 6-Custom providers, installed or created
        //REF 7-Directory files
        //REF 8-In - memory.NET objects
        //REF 9-Third - party providers


        //test 'Command line arguments' and 'Environments vars'
        //Visual Studio:
        //right click on project -> Properties
        //on left menu, choose -> debug
        //Debug : General, click on -> 'Open debug launch profiles UI'
        //now we can add;
        //1 - Command line arguments
        //2 - Environments vars

        //all this information is put on file: Properties\launchSettings.json


        public static void Main(string[] args)
        {
            ReadAppConfigXmlFileLegacy();
            ReadAppConfigJsonFileLegary();

            ReadAppXmlFile();
            ReadAppJsonFile();
            ReadAppIniFile();
            ReadAppYamlFile();

            ReadEnvironmentVariables();

            ReadCommandLine(args);


            //IConfiguration configuration = new ConfigurationBuilder()
            // .SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile(path: "appsettings-legacy.json", optional: false, reloadOnChange: true) //REF 1
            //.AddEnvironmentVariables() // or.AddEnvironmentVariables("Specific Var") //REF 2 
            //.AddCommandLine(args) //REF 5
            //.AddInMemoryCollection() //REF 8
            //.AddXmlFile(path: "app.config", optional: false, reloadOnChange: true) //REF 1
            //.AddIniFile(path: "app.ini", optional: false, reloadOnChange: true) //REF 1)
            //.Build();

             Console.ReadLine();
        }

        #region "Actions"

        private static void ReadAppConfigXmlFileLegacy()
        {
            //Add
            //<PackageReference Include="System.Configuration.ConfigurationManager" />

            //'conflit' with Namespaces over class 'ConfigurationManager'
            //'System.Configuration.ConfigurationManager' vs 'Microsoft.Extensions.Configuration'
            //

            string example1 = System.Configuration.ConfigurationManager.AppSettings["Example1"];
            string example2 = System.Configuration.ConfigurationManager.AppSettings["Example2"];
            string temp = System.Configuration.ConfigurationManager.AppSettings["ExampleBool"];
            bool exanpleBool = System.Convert.ToBoolean(temp);
            temp = System.Configuration.ConfigurationManager.AppSettings["ExampleInt"];
            int exanpleInt = System.Convert.ToInt32(temp);

            System.Configuration.ConnectionStringSettings conn1 = System.Configuration.ConfigurationManager.ConnectionStrings["Conn1"];
            string conn1Name = conn1.Name;
            string conn1Provider = conn1.ProviderName;
            string conn1String = conn1.ConnectionString;
        }

        private static void ReadAppXmlFile()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile(path: "app.config", optional: false, reloadOnChange: true) //REF 1
                .Build();

            ////////////////////////////////

            //ok
            string conn1a = configuration["connectionStrings:add:Conn1:name"];
            string conn1b = configuration["connectionStrings:add:Conn1:providerName"];
            string conn1c = configuration["connectionStrings:add:Conn1:connectionString"];
            string conn2a = configuration["connectionStrings:add:Conn2:name"];
            string conn2b = configuration["connectionStrings:add:Conn2:providerName"];
            string conn2c = configuration["connectionStrings:add:Conn2:connectionString"];

            //ok
            string key0 = configuration["appSettings:add:0:key"];
            string value0 = configuration["appSettings:add:0:value"];
            string key1 = configuration["appSettings:add:1:key"];
            string value1 = configuration["appSettings:add:1:value"];


            //not ok
            string lifetimeKeyx = configuration["configuration:appSettings:Example1"]; //ko
            string key0a = configuration["appSettings:add:Example1:key"]; //ko
            string lifetimeKeyy = configuration["appSettings:Example1"]; //ko
            string key0y = configuration["appSettings:add:Example1:value"]; //ko

        }

        private static void ReadAppJsonFile()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true) //REF 1
                .Build();

            string sqlConn = configuration["ConnectionStrings:MySqliteConnectionNo"];
            string lifetime1 = configuration["Logging:LogLevel:Microsoft.Hosting.Lifetime"];
            string title = configuration["Title"];

        }

        private static void ReadAppConfigJsonFileLegary()
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings-legacy.json", optional: false, reloadOnChange: true) //REF 1
                .Build();

            string sqlConn1 = configuration.GetConnectionString("MyDb1");
            string sqlConn2 = configuration.GetConnectionString("MyDb2:connectionString");
            //or
            string sqlConn1b = configuration["ConnectionStrings:MyDb1"];
            string sqlConn2b = configuration["ConnectionStrings:MyDb2:connectionString"];
            string sqlConn2c = configuration["ConnectionStrings:MyDb2:providerName"];

            string lifetime1 = configuration["apiSettings:url"];

        }

        private static void ReadAppIniFile()
        {
            IConfiguration configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddIniFile(path: "App.ini", optional: false, reloadOnChange: true)
          .Build();

            Console.Clear();

            string sqlConn1 = configuration["Conn1:connectionString"];
            string sqlConn1p = configuration["Conn1:providerName"];

        }

        private static void ReadAppYamlFile()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile(path: "Example1.yaml", optional: false, reloadOnChange: true)
                .Build();


            string aa0 = configuration["doe"];
            string aa1 = configuration["xmas-fifth-day:calling-birds"];
            string aa2 = configuration["xmas-fifth-day:french-hens"];
            string aa3 = configuration["xmas-fifth-day:partridges:location"];

        }

        private static void ReadEnvironmentVariables()
        {
            //Get-ChildItem -Path Env:
            //Get-ChildItem -Path Env:\SystemDrive

            //MS-DOS entries with __ between words generate sub level
            //xpto__config1 = "configValue1"
            //xptp__config2 = "configValue2"
            //is equals to
            //xpto:
            //  config1: "configValue1"
            //  config2: "configValue2"

            string val1 = Environment.GetEnvironmentVariable("USERNAME");
            string val2 = Environment.GetEnvironmentVariable("USERPROFILE");
            string val3 = Environment.GetEnvironmentVariable("Zulu__Alfa");
            string val4 = Environment.GetEnvironmentVariable("Zulu__Beta");

            //

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables() // or.AddEnvironmentVariables("Specific Var") //REF 2 
                .Build();

            string val1b = configuration["USERNAME"];
            string val2b = configuration["USERPROFILE"];
            string val3b = configuration["Zulu:Alfa"];
            string val4b = configuration["Zulu:Beta"];
        }

        private static void ReadCommandLine(string[] args)
        {
            string val = Environment.CommandLine;

            //note: all args[] are split by space

            //generate keyPair
            IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddCommandLine(args) //REF 5
            .Build();

            //examples:
            //dotnet run key1=value1 --key2=value2 /key3=value3 --key4 value4 /key5 value5
            //app.exe key1=value1 --key2=value2 /key3=value3 --key4 value4 /key5 value5
            //note: all arguments must a pair, arguments like: 'alfa bravo charley' are ignored

            string val1 = configuration["Key1"];
            string val2 = configuration["Key2"];
            string val3 = configuration["Key3"];
            string val4 = configuration["Key4"];
            string val5 = configuration["Key5"];

        }

        #endregion

        #region "Other Attempts"

        private static IConfiguration SetupConfiguration(string[] args)
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())

                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true) //REF 1
                .AddEnvironmentVariables() // or.AddEnvironmentVariables("Specific Var") //REF 2 
                .AddCommandLine(args) //REF 5
                .AddInMemoryCollection() //REF 8
                .AddXmlFile(path: "app.config", optional: false, reloadOnChange: true) //REF 1
                .AddIniFile(path: "app.ini", optional: false, reloadOnChange: true) //REF 1)
                .Build();

            OtherTestsConfigurationExtraInfo(configuration);

            return configuration;
        }

        private static void OtherTestsConfigurationExtraInfo(IConfiguration configuration)
        {
            //https://www.youtube.com/watch?v=dwMFg6uxQ0I&t=2687s

            //extra info
            Microsoft.Extensions.Configuration.ConfigurationRoot configurationRoot =
                    configuration as Microsoft.Extensions.Configuration.ConfigurationRoot;

            IEnumerable<IConfigurationProvider> providers = configurationRoot.Providers;
            //List<IConfigurationProvider> providers = configurationRoot.Providers as List<IConfigurationProvider>;

            foreach (IConfigurationProvider provider in providers)
            {
                Console.WriteLine("============================================");
                Console.WriteLine(provider.GetType().Name);
                Console.WriteLine("============================================");

                IEnumerable<string> keys = provider.GetChildKeys(new string[0], null);
                string value;
                foreach (string key in keys)
                {
                    //  provider.
                    provider.TryGet(key, out value);
                    Console.WriteLine($"Key:{key} | Value: {value}");
                }

                switch (provider)
                {
                    case JsonConfigurationProvider:
                        break;

                    case EnvironmentVariablesConfigurationProvider:
                        EnvironmentVariablesConfigurationProvider a1 = provider as EnvironmentVariablesConfigurationProvider;
                        IEnumerable<string> xx1 = a1.GetChildKeys(new string[0], null);
                        break;

                    case CommandLineConfigurationProvider:
                        //CommandLineConfigurationProvider a1 = provider as CommandLineConfigurationProvider;
                        //IEnumerable<string> xx1 = a1.GetChildKeys(new string[0], null);
                        break;

                    case MemoryConfigurationProvider:
                        break;

                    case IniConfigurationProvider:
                        break;

                    case XmlConfigurationProvider:
                        XmlConfigurationProvider a1x = provider as XmlConfigurationProvider;
                        IEnumerable<string> xx1x = a1x.GetChildKeys(new string[0], null);
                        string myOut;
                        a1x.TryGet("appSettings:add:0:key", out myOut);
                        a1x.TryGet("", out myOut);
                        break;

                    default:
                        break;
                }
            }

            #endregion

        }
    }
}
