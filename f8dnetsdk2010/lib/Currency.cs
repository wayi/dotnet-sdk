using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace f8dnetsdk2010
{
    public class Currency
    {
        // You can find the following functions and more details
        // on http://developers.facebook.com/docs/authentication/canvas.
        public static JObject parse_signed_request(String signed_request, String secret)
        {
            if (signed_request == null)
                return null;

            String[] str_array = signed_request.Split(new Char[] { '.' }, 2);
            String encoded_sig = str_array[0];
            String payload = str_array[1];

            //
            // Decode the data
            String sig = base64Decode(encoded_sig);

            String testtt = base64Decode(payload);
            JObject data = JObject.Parse(base64Decode(payload));

            if (data["algorithm"].ToString().ToUpper() != "HMAC-SHA256")
            {
                //error_log('Unknown algorithm. Expected HMAC-SHA256');
                return null;
            }

            // Check signature
            String expected_sig = hash_hmac_sha256(payload, secret);

            if (sig != expected_sig)
            {
                //error_log('Bad Signed JSON signature!');
                return null;
            }

            return data;
        }

        public static String hash_hmac_sha256(String message, String secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();

            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                char[] decoded_char = encoding.GetChars(hashmessage);
                string result = new String(decoded_char);
                return result;
            }
        }

        public static String base64Decode(String data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                String result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        public static String make_error_report(String message, int code = 500)
        {
            JObject error = new JObject();
            error["code"] = code;
            error["msg"] = message;
            JObject result = new JObject();
            result["error"] = error;

            return result.ToString();
        }
    }
}