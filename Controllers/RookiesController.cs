using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ass_5.Models;
using System.Globalization;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace Ass_5.Controllers;

public class RookiesController : Controller
{
    static List<Person> persons = new List<Person>
        {
            new Person
            {
                FirstName = "Phuong",
                LastName = "Nguyen Nam",
                Gender = "Male",
                DateOfBirth = new DateTime(2001, 1, 22),
                PhoneNumber = "",
                BirthPlace = "Phu Tho",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Nam",
                LastName = "Nguyen Thanh",
                Gender = "Male",
                DateOfBirth = new DateTime(2001, 1, 20),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Son",
                LastName = "Do Hong",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 11, 6),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Huy",
                LastName = "Nguyen Duc",
                Gender = "Male",
                DateOfBirth = new DateTime(1996, 1, 26),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Hoang",
                LastName = "Phuong Viet",
                Gender = "Male",
                DateOfBirth = new DateTime(1999, 2, 5),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Long",
                LastName = "Lai Quoc",
                Gender = "Male",
                DateOfBirth = new DateTime(1997, 5, 30),
                PhoneNumber = "",
                BirthPlace = "Bac Giang",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Thanh",
                LastName = "Tran Chi",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 9, 18),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Manh",
                LastName = "Le Duc",
                Gender = "Male",
                DateOfBirth = new DateTime(2001,4, 22),
                PhoneNumber = "",
                BirthPlace = "Ha Noi",
                IsGraduated = false
            },
            new Person
            {
                FirstName = "Dung",
                LastName = "Dao Tan",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 12, 7),
                PhoneNumber = "",
                BirthPlace = "Hung Yen",
                IsGraduated = false
            }
        };

    // public IActionResult Index(){
    //     return View(persons);
    // }
    
    public IActionResult GetMaleMembers(){
        var result = from person in persons
                    where person.Gender == "Male"
                    select person;
        // return new ObjectResult(result);
        return Json(result);
    }

    
    public IActionResult GetOldestMember(){
        var maxAge = persons.Max(m => m.Age);
        var oldest = persons.First(m =>m.Age == maxAge);
        return new ObjectResult(oldest);
    }

    
    public IActionResult GetFullNames(){
        var fullnames = persons.Select(m => m.FullName);
        return new ObjectResult(fullnames);
    }

    
    public IActionResult SplitMemberByYear(int year){
        var results = from person in persons
                    group person by person.DateOfBirth.Year.CompareTo(year) into grp
                    select new 
                    {
                        Key = grp.Key switch
                        {
                            -1 => $"Birth year less than {year}",
                            0 => $"Birth year equals to {year}",
                            1 => $"Birth year greater than {year}",
                            _ => string.Empty
                        },
                        Data = grp.ToList()
                    };
        return Json(results);
    }

    
    public IActionResult Export()
    {
        var buffer = WriteCsvToMemory(persons); 
        var memoryStream = new MemoryStream(buffer);
        return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = "member.csv" };
    }
     public byte[] WriteCsvToMemory(List<Person> data)
    {
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream))
        using (var csvWriter = new CsvWriter(writer , CultureInfo.InvariantCulture))
        {
            var options = new TypeConverterOptions { Formats = new[] { "dd/MM/yyyy" } };
            csvWriter.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);

            csvWriter.WriteRecords(data);
            writer.Flush();
            return stream.ToArray();
        }
    }


}
