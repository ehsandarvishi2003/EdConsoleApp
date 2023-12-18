using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class Subject
{
    public string SubjectId { get; set; }
    public string Title { get; set; }
    public string Parent { get; set; }
    public List<Subject> Children { get; set; }

    public Subject()
    {
        Children = new List<Subject>();
    }
}

public class FileOperations
{
    public List<Subject> ReadJsonFile(string filePath)
    {
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            List<Subject> data = JsonConvert.DeserializeObject<List<Subject>>(jsonContent);
            return data;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found!");
            return null;
        }
        catch (JsonException)
        {
            Console.WriteLine("Invalid JSON format!");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public void SaveTreeAsJson(List<Subject> treeData, string filePath)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(treeData, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
            Console.WriteLine("درخت با موفقیت در فایل ذخیره شد.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"خطا در ذخیره فایل: {ex.Message}");
        }
    }

    public List<Subject> BuildTree(List<Subject> data, string parentId = null)
    {
        List<Subject> subjects = new List<Subject>();

        foreach (var item in data.Where(x => x.Parent == parentId))
        {
            Subject subject = new Subject
            {
                SubjectId = item.SubjectId,
                Title = item.Title,
                Parent = item.Parent
            };

            subject.Children.AddRange(BuildTree(data, item.SubjectId));
            subjects.Add(subject);
        }

        return subjects;
    }
}

class Program
{
    static void Main()
    {
        // فایل JSON ورودی
        string inputFilePath = @"E:\repo\IDP\EdConsoleApp\EdConsoleApp\subject.json";

        FileOperations fileOps = new FileOperations();

        // خواندن محتوای فایل JSON و ساختاردهی آن به عنوان یک درخت
        List<Subject> inputData = fileOps.ReadJsonFile(inputFilePath);

        if (inputData != null)
        {
            // ساختاردهی ساختار درختی
            List<Subject> treeData = fileOps.BuildTree(inputData);

            // مسیر و نام فایل برای ذخیره ساختار درختی
            string outputFilePath = @"E:\repo\IDP\EdConsoleApp\EdConsoleApp\result.json";

            // ذخیره ساختار درختی به صورت فایل JSON
            fileOps.SaveTreeAsJson(treeData, outputFilePath);
        }
    }
}
