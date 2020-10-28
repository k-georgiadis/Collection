using bundleSVC.Models;
using System.Collections.Generic;

namespace bundleSVC.Data
{
    public interface IBundleRepo
    {
        bool SaveChanges();

        IEnumerable<Bundle> GetAllBundles();
        Bundle GetBundleByCode(int code);
        IEnumerable<Bundle> GetBundleByName(string name);
        IEnumerable<Bundle> GetBundleByPrice(float price, string condition, string order);

        void AddBundle(Bundle _bundle);
        void UpdateBundle(Bundle _bundle);
        void DeleteBundle(Bundle _bundle);
    }
}
