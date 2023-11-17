using AutoMapper;

namespace POS.API.Helpers.Mapping
{
    public static class MapperConfig
    {
        public static IMapper GetMapperConfigs()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ActionProfile());
                mc.AddProfile(new PageProfile());
                mc.AddProfile(new RoleProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new NLogProfile());
                mc.AddProfile(new EmailTemplateProfile());
                mc.AddProfile(new EmailProfile());
                mc.AddProfile(new CountryProfile());
                mc.AddProfile(new CustomerProfile());
                mc.AddProfile(new TestimonialsProfile());
                mc.AddProfile(new NewsletterSubscriberProfile());
                mc.AddProfile(new CityProfile());
                mc.AddProfile(new SupplierProfile());
                mc.AddProfile(new ContactUsMapping());

                mc.AddProfile(new ReminderProfile());
                mc.AddProfile(new PurchaseOrderProfile());
                mc.AddProfile(new SalesOrderProfile());

                mc.AddProfile(new CompanyProfileProfile());
                mc.AddProfile(new ExpenseProfile());
                mc.AddProfile(new CurrencyProfile());
                mc.AddProfile(new UnitProfile());
                mc.AddProfile(new TaxProfile());
                mc.AddProfile(new WarehouseProfile());


                mc.AddProfile(new InquiryNoteProfile());
                mc.AddProfile(new InquiryActivityProfile());
                mc.AddProfile(new InquiryAttachmentProfile());
                mc.AddProfile(new InquiryProfile());
                mc.AddProfile(new InquiryStatusProfile());
                mc.AddProfile(new InquirySourceProfile());

                mc.AddProfile(new ProductCategoryProfile());
                mc.AddProfile(new ProductProfile());

                mc.AddProfile(new BrandProfile());
                mc.AddProfile(new CounterProfile());
                mc.AddProfile(new InventoryProfle());
                mc.AddProfile(new CartProfile());
                mc.AddProfile(new CustomerAddressProfile());
                mc.AddProfile(new WishlistProfile());
            });
            return mappingConfig.CreateMapper();
        }
    }
}
