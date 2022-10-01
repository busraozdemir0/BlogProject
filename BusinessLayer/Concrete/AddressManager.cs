using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AddressManager: IAddressService
    {
        IAddressDal _addressDal;

        public AddressManager(IAddressDal addressDal)
        {
            _addressDal = addressDal;
        }

        public List<Address> GetList()
        {
            return _addressDal.GetListAll();
        }

        public void TAdd(Address t)
        {
            _addressDal.Insert(t);
        }

        public void TDelete(Address t)
        {
            _addressDal.Delete(t);
        }

        public Address TGetById(int id)
        {
            return _addressDal.GetByID(id);
        }

        public void TUpdate(Address t)
        {
            _addressDal.Update(t);
        }
    }
}
