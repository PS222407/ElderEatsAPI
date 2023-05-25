using System.ComponentModel.DataAnnotations;
using ElderEatsAPI.Enums;

namespace ElderEatsAPI.Dto;

public class AccountUserDto
{
    [EnumDataType(typeof(ConnectionStatus))]
    public ConnectionStatus Status { get; set; }
}