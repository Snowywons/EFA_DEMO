using EFA_DEMO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

public static class DBEntitiesExtensionsMethods
{
    public static UserView ToUserView(this User user)
    {
        return new UserView()
        {
            Id = user.Id,
            AvatarId = user.AvatarId,
            UserName = user.UserName,
            FullName = user.FullName,
            Password = user.Password,
            Admin = user.Admin
        };
    }

    public static PostView ToPostView(this Post post)
    {
        return new PostView()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Tags = post.Tags,
            CreationDate = post.CreationDate,
            UserId = post.UserId,
            User = post.User.ToUserView()
        };
    }

    public static bool UserNameExist(this DBEntities DB, string userName)
    {
        User user = DB.Users.Where(u => u.UserName == userName).FirstOrDefault();
        return (user != null);
    }

    public static User FindByUserName(this DBEntities DB, string userName)
    {
        User user = DB.Users.Where(u => u.UserName == userName).FirstOrDefault();
        return user;
    }

    public static UserView AddUser(this DBEntities DB, UserView userView)
    {
        userView.SaveAvatar();
        User user = userView.ToUser();
        user = DB.Users.Add(user);
        DB.SaveChanges();
        return user.ToUserView();
    }

    public static bool UpdateUser(this DBEntities DB, UserView userView)
    {
        userView.SaveAvatar();
        User userToUpdate = DB.Users.Find(userView.Id);
        userView.CopyToUser(userToUpdate);
        DB.Entry(userToUpdate).State = EntityState.Modified;
        DB.SaveChanges();
        return true;
    }

    public static bool RemoveUser(this DBEntities DB, UserView userView)
    {
        userView.RemoveAvatar();
        User userToDelete = DB.Users.Find(userView.Id);
        DB.Users.Remove(userToDelete);
        DB.SaveChanges();
        return true;
    }

    public static Log AddUserLog(this DBEntities DB, UserView userView)
    {
        Log log = new Log();
        log.UserId = userView.Id;
        log.LoginDate = DateTime.Now;
        DB.Logs.Add(log);
        DB.SaveChanges();
        return log;
    }
}
