using System;
using JetBrains.Annotations;
using LegacyApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyApp.Tests;

[TestClass]
[TestSubject(typeof(UserService))]
public class UserServiceTest {

    [TestMethod]
    public void AddUser_Should_Return_False_When_User_Has_Credit_Limit_Under_500()
    {
        string firstName = "Jan";
        string lastName = "Kowalski";
        DateTime birthDate = new DateTime(1997, 4, 18);
        int clientId = 1;
        string email = "kowalski@wp.pl";
        var service = new UserService();

        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        Assert.AreEqual(false, result);
    }
    
    [TestMethod]
    public void AddUser_Should_Return_False_When_User_Has_Credit_Limit_Above_500()
    {
        string firstName = "John";
        string lastName = "Doe";
        DateTime birthDate = new DateTime(1968, 2, 29);
        int clientId = 4;
        string email = "JoDoe@gmail.com";
        var service = new UserService();

        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        Assert.AreEqual(true, result);
    }
    
    [TestMethod]
    public void AddUser_Should_Return_False_When_User_Is_Very_Important_Client()
    {
        string firstName = "Mateusz";
        string lastName = "Malewski";
        DateTime birthDate = new DateTime(2003, 9, 29);
        int clientId = 2;
        string email = "malewski@gmail.com";
        var service = new UserService();

        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        Assert.AreEqual(false, result);
    }
    
    [TestMethod]
    public void AddUser_Should_Return_False_When_User_Has_No_Name_Or_Last_Name()
    {
        string firstName = "Jan";
        string lastName = "";
        DateTime birthDate = new DateTime(1988, 7, 10);
        int clientId = 1;
        string email = "Jan@wp.pl";
        var service = new UserService();

        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        Assert.AreEqual(false, result);
    }
}