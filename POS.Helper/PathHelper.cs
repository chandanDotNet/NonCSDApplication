using Microsoft.Extensions.Configuration;
using System.IO;

namespace POS.Helper
{
    public class PathHelper
    {
        public IConfiguration _configuration;

        public PathHelper(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string UserProfilePath
        {
            get
            {
                return _configuration["UserProfilePath"];
            }
        }

        public string BrandImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:BrandImages"];
            }
        }

        public string ProductImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:ProductImages"];
            }
        }

        public string ProductThumbnailImagePath
        {
            get
            {
                return Path.Combine(ProductImagePath, "Thumbnail");
            }
        }

        public string NoImageFound
        {
            get
            {
                return _configuration["ImagePathSettings:NoImageFound"];
            }
        }

        public string CompanyLogo
        {
            get
            {
                return _configuration["ImagePathSettings:CompanyLogo"];
            }
        }

        public string SupplierImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:SupplierImages"];
            }
        }

        public string ArticleBannerImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:ArticleBannerImagePath"];
            }
        }

        public string CustomerImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:CustomerImages"];
            }
        }

        public string TestimonialsImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:TestimonialsImagePath"];
            }
        }

        public string Attachments
        {
            get
            {
                return _configuration["ImagePathSettings:Attachments"];
            }
        }

        public string SiteMapPath
        {
            get
            {
                return _configuration["SiteMapPath"];
            }
        }

        public string DocumentPath
        {
            get
            {
                return _configuration["DocumentPath"];
            }
        }
        public string AesEncryptionKey
        {
            get
            {
                return _configuration["AesEncryptionKey"];
            }
        }

        public string ReminderFromEmail
        {
            get
            {
                return _configuration["ReminderFromEmail"];
            }
        }
    }
}
