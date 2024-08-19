public class InputValidator
{
    public static bool ValidateInteger(string input, out int result, int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        if (int.TryParse(input, out result))
        {
            return result >= minValue && result <= maxValue;
        }
        return false;
    }

    public static bool ValidateDecimal(string input, out decimal result, decimal minValue = decimal.MinValue, decimal maxValue = decimal.MaxValue)
    {
        if (decimal.TryParse(input, out result))
        {
            return result >= minValue && result <= maxValue;
        }
        return false;
    }

    public static bool ValidateString(string input, int minLength = 0, int maxLength = int.MaxValue)
    {
        return !string.IsNullOrWhiteSpace(input) && input.Length >= minLength && input.Length <= maxLength;
    }

    public static bool ValidateEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
