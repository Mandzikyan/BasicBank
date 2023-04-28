using FCBankBasicHelper.Attributes;

namespace Models.DTO
{
    public partial class PhoneModel
    {
        public int UserId { get; set; }
        [Property]
        public string PhoneNumber { get; set; } = null!;

        public int Id { get; set; }

    }
}