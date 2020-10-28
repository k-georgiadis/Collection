using bundleSVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bundleSVC.Data
{
    public class SqlBundleRepo : IBundleRepo
    {
        private readonly BundleContext _context;

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public SqlBundleRepo(BundleContext context)
        {
            _context = context;
        }

        public IEnumerable<Bundle> GetAllBundles()
        {
            return _context.Bundles;
        }

        public Bundle GetBundleByCode(int code)
        {
            return _context.Bundles.FirstOrDefault(b => b.B_code == code);
        }

        public IEnumerable<Bundle> GetBundleByName(string name)
        {
            return _context.Bundles.Where(b => b.B_name == name);
        }

        public IEnumerable<Bundle> GetBundleByPrice(float price, string condition, string order)
        {
            IQueryable<Bundle> bundles;

            //Check if a valid condition is given.
            if (condition == "<=")
                bundles = _context.Bundles.Where(b => b.B_price <= price);
            else if (condition == ">=")
                bundles = _context.Bundles.Where(b => b.B_price >= price);
            else if (condition == "==")
                bundles = _context.Bundles.Where(b => b.B_price == price);
            else
                return Enumerable.Empty<Bundle>(); //Return empty list on invalid condition.

            //Check if a valid order is given.
            if (order == "ASC")
                bundles = bundles.OrderBy(b => b.B_price);
            else if (order == "DESC")
                bundles = bundles.OrderByDescending(b => b.B_price);
            else
            {
                //Return empty list on invalid order, except on equality condition.
                if (condition != "==")
                    return Enumerable.Empty<Bundle>();
            }

            //Return the sorted bundles.
            return bundles;
        }

        public void AddBundle(Bundle _bundle)
        {
            if (_bundle == null)
                throw new ArgumentNullException(nameof(_bundle));
            else
                _context.Bundles.Add(_bundle);
        }

        public void DeleteBundle(Bundle _bundle)
        {
            if (_bundle == null)
                throw new ArgumentNullException(nameof(_bundle));
            else
                _context.Bundles.Remove(_bundle);
        }
        //For future purposes.
        public void UpdateBundle(Bundle _bundle)
        {
            //Nothing.
        }
    }
}
