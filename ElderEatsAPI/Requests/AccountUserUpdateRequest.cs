using System.ComponentModel.DataAnnotations;
using ElderEatsAPI.Enums;

namespace ElderEatsAPI.Requests;

public class AccountUserUpdateRequest
{
    [EnumDataType(typeof(ConnectionStatus))]
    public ConnectionStatus Status { get; set; }
}