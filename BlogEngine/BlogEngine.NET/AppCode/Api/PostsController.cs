using App_Code;
using BlogEngine.Core;
using BlogEngine.Core.Data.Contracts;
using BlogEngine.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;


public class PostsController : ApiController
{
    readonly IPostRepository repository;

    public PostsController(IPostRepository repository)
    {
        this.repository = repository;
    }

    public IEnumerable<PostItem> Get(int take = 10, int skip = 0, string filter = "", string order = "")
    {
        return repository.Find(take, skip, filter, order);
    }

    public HttpResponseMessage Get(string id)
    {
        var result = repository.FindById(new Guid(id));
        if (result == null)
            return Request.CreateResponse(HttpStatusCode.NotFound);

        return Request.CreateResponse(HttpStatusCode.OK, result);
    }

    public HttpResponseMessage Post([FromBody]PostDetail item)
    {
        var result = repository.Add(item);
        if (result == null)
            return Request.CreateResponse(HttpStatusCode.NotModified);

        // Regenerate Sitemap
        generateSitemap();

        return Request.CreateResponse(HttpStatusCode.Created, result);
    }

    [HttpPut]
    public HttpResponseMessage Update([FromBody]PostDetail item)
    {
        repository.Update(item, "update");

        // Regenerate Sitemap
        generateSitemap();
        
        return Request.CreateResponse(HttpStatusCode.OK);
    }
	
	[HttpPut]
    public HttpResponseMessage ProcessChecked([FromBody]List<PostDetail> items)
    {
        if (items == null || items.Count == 0)
            throw new HttpResponseException(HttpStatusCode.ExpectationFailed);

        var action = Request.GetRouteData().Values["id"].ToString().ToLowerInvariant();
      
        foreach (var item in items)
        {
            if (item.IsChecked)
            {
                if (action == "delete")
                {
                    repository.Remove(item.Id);
                }
                else
                {
                    repository.Update(item, action);
                }	
            }
        }

        return Request.CreateResponse(HttpStatusCode.OK);
    }

    public HttpResponseMessage Delete(string id)
    {
        Guid gId;
        if (Guid.TryParse(id, out gId))
            repository.Remove(gId);

        // Regenerate Sitemap
        generateSitemap();

        return Request.CreateResponse(HttpStatusCode.OK);
    }

    /// <summary>
    /// Generates and stores the sitemap - everytime we modify / delete / add a post
    /// </summary>
    private void generateSitemap()
    {
        // Take published posts
        var posts = repository.Find().Where(x => x.IsPublished == true).ToList();

        // Create the file : text sitemap
        StringBuilder sb = new StringBuilder();
        // Get the host
        var host = Utils.AbsoluteWebRoot.Scheme + "://" + Utils.AbsoluteWebRoot.Host;
        foreach (var post in posts)
            sb.AppendLine($"{host}{post.RelativeLink}");

        // Stores the file
        var inputDirectory = HttpContext.Current.Server.MapPath("~");
        var fileName = Path.Combine(inputDirectory, "Sitemap", "sitemap.txt");
        FileStream fParameter = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        StreamWriter m_WriterParameter = new StreamWriter(fParameter);
        m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
        m_WriterParameter.Write(sb.ToString());
        m_WriterParameter.Flush();
        m_WriterParameter.Close();
    }
}