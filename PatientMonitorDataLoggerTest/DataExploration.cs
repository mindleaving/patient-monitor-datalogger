﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLoggerTest;

/// <summary>
/// Methods for exploring Philips Intellivue communication data
/// </summary>
public class DataExploration
{
    [Test]
    [TestCase("BakGFwaaBywHxAhZCOUJYQnKCh4KXgqMCqkKtwq2CqgKjwpsCj8KCgnNCYkJPgjuCJwISgf7B7IHbwc2BwcG5A==")]
    public void DeconstructObservedValueArray(
        string dataString)
    {
        var data = Convert.FromBase64String(dataString);
        foreach (var bPair in data.Chunk(2))
        {
            var value = BigEndianBitConverter.ToUInt16(bPair);
            Console.WriteLine($"{value}");
        }
    }

    [Test]
    public void DebugFaultyFrameWithPatientDemographicsResult()
    {
        var data = Convert.FromBase64String("wBEBASThAAACAAIBHAAAAAcBFgAhAAAAAAwWAQwAAAWe8gD//////////wABACoIBwABAPQAAAABAO4AUAAaAOgJIQACAFAJVwACAAIKGgACIADwAQAKyNnSEQ40UpooXAliAAIAAwoeAAIAAAldAAQAAgAACV8ABAACAAAJXAAQAA4ASwBvAGUAbgBpAGcAAPLUAAQAAgAACVoACgAIADEAMgAzAADy4QAEAAIAAPEpAAQAAgAA8SoABAACAAAJYQACAAIJWAAIAAAAAAAAAAAJ2AAGAH///wlACdwABgB///8FEQnfAAYAf///BsAJVgAGAH///wXA8ewAAgAB+egACAAAAAAAAAAA+ekAAvEL84oABAAAAADy4gAEgHUglfLjAASAiw2ZgmXB");
        var frame = Rs232Frame.Parse(data);
        Console.WriteLine(frame.UserData.ToString());
        var jsonSerializerSettings = new JsonSerializerSettings { Converters = { new StringEnumConverter() }};
        Console.WriteLine(JsonConvert.SerializeObject(frame.UserData, Formatting.Indented, jsonSerializerSettings));
    }

    [Test]
    public void ConvertSampleDataToBase64()
    {
        var data = new ushort[]
        {
            8189,
8185,
8182,
8180,
8179,
8178,
8179,
8178,
8174,
8169,
8164,
8159,
8157,
8158,
8160,
8163,
8166,
8169,
8174,
8177,
8180,
8182,
8182,
8183,
8189,
8192,
8191,
8191,
8197,
8198,
8195,
8192,
8193,
8192,
8187,
8178,
8176,
8195,
8231,
8271,
8326,
8417,
8550,
8702,
8827,
8829,
8658,
8415,
8219,
8108,
8066,
8061,
8069,
8089,
8123,
8170,
8211,
8235,
8247,
8254,
8259,
8261,
8262,
8261,
8260,
8263,
8269,
8273,
8273,
8271,
8270,
8272,
8277,
8279,
8279,
8282,
8287,
8294,
8301,
8304,
8301,
8295,
8293,
8293,
8296,
8298,
8298,
8298,
8302,
8312,
8321,
8323,
8325,
8328,
8328,
8330,
8338,
8345,
8352,
8358,
8366,
8379,
8394,
8406,
8414,
8422,
8435,
8447,
8462,
8479,
8497,
8517,
8533,
8544,
8556,
8568,
8585,
8600,
8608,
8612,
8621,
8629,
8631,
8625,
8614,
8603,
8594,
8577,
8550,
8514,
8478,
8449,
8424,
8395,
8367,
8341,
8317,
8294,
8272,
8255,
8243,
8232,
8222,
8213,
8205,
8197,
8190,
8186,
8183,
8180,
8178,
8178,
8182,
8186,
8186,
8181,
8177,
8172,
8170,
8168,
8166,
8167,
8169,
8170,
8169,
8174,
8181,
8185,
8187,
8183,
8177,
8175,
8176,
8178,
8185,
8187,
8185,
8180,
8179,
8177,
8172,
8167,
8164,
8161,
8161,
8161,
8166,
8171,
8171,
8170,
8169,
8166,
8162,
8158,
8153,
8150,
8150,
8153,
8156,
8154,
8155,
8162,
8169,
8172,
8170,
8166,
8164,
8159,
8155,
8157,
8163,
8164,
8163,
8167,
8172,
8173,
8173,
8174,
8175,
8169,
8165,
8165,
8167,
8171,
8174,
8170,
8165,
8158,
8155,
8156,
8158,
8157,
8158,
8164,
8171,
8177,
8181,
8182,
8185,
8188,
8185,
8177,
8171,
8168,
8166,
8165,
8168,
8174,
8182,
8189,
8196,
8198,
8197,
8196,
8196,
8194,
8192,
8193,
8195,
8192,
8186,
8183,
8184,
8187,
8192,
8190,
8188,
8187,
8192,
8194,
8192,
8188,
8186,
8186,
8188,
8191,
8196,
8200,
8199,
8196,
8192,
8192,
8193,
8191,
8188,
8190,
8196,
8197,
8194,
8191,
8190,
8187,
8184,
8183,
8185,
8190,
8192,
8194,
8196,
8196,
8198,
8196,
8192,
8191,
8191,
8192,
8195,
8198,
8198,
8194,
8192,
8192,
8194,
8195,
8195,
8196,
8198,
8198,
8198,
8200,
8207,
8215,
8221,
8224,
8225,
8229,
8235,
8238,
8243,
8252,
8266,
8271,
8267,
8261,
8255,
8249,
8240,
8231,
8219,
8203,
8189,
8178,
8171,
8170,
8178,
8181,
8176,
8170,
8166,
8166,
8172,
8176,
8179,
8180,
8182,
8183,
8183,
8184,
8185,
8186,
8186,
8188,
8192,
8192,
8191,
8190,
8191,
8192,
8193,
8193,
8194,
8195,
8197,
8199,
8200,
8198,
8193,
8191,
8198,
8219,
8259,
8321,
8421,
8551,
8692,
8821,
8872,
8757,
8524,
8304,
8170,
8122,
8118,
8117,
8120,
8138,
8175,
8219,
8253,
8269,
8278,
8286,
8293,
8294,
8294,
8289,
8286,
8284,
8282,
8285,
8290,
8290,
8288,
8288,
8291,
8292,
8293,
8294,
8295,
8298,
8303,
8307,
8310,
8314,
8320,
8325,
8327,
8327,
8327,
8329,
8333,
8339,
8345,
8350,
8354,
8360,
8366,
8371,
8379,
8388,
8393,
8397,
8406,
8419,
8431,
8441,
8453,
8466,
8482,
8497,
8511,
8527,
8546,
8562,
8575,
8588,
8604,
8620,
8635,
8647,
8662,
8670,
8669,
8665,
8663,
8659,
8651,
8634,
8613,
8586,
8558,
8526,
8492,
8456,
8424,
8397,
8376,
8352,
8327,
8308,
8294,
8282,
8267,
8252,
8241,
8231,
8227,
8225,
8228,
8230,
8231,
8231,
8229,
8228,
8225,
8220,
8219,
8221,
8225,
8228,
8230,
8232,
8235,
8235,
8234,
8235,
8236,
8232,
8225,
8220,
8217,
8214,
8213,
8212,
8210,
8208,
8206,
8204,
8204,
8202,
8200,
8198,
8195,
8194,
8194,
8194,
8196,
8196,
8194,
8189,
8182,
8178,
8180,
8182,
8183,
8185,
8187,
8190,
8193,
8192,
8186,
8179,
8174,
8172,
8174,
8177,
8183,
8188,
8190,
8189,
8187,
8183,
8178,
8174,
8171,
8173,
8176,
8177,
8177,
8175,
8174,
8174,
8176,
8176,
8175,
8176,
8177,
8178,
8181,
8185,
8186,
8188,
8187,
8183,
8179,
8178,
8183,
8189,
8192,
8187,
8181,
8180,
8184,
8187,
8188,
8187,
8189,
8193,
8196,
8197,
8195,
8193,
8192,
8192,
8193,
8195,
8199,
8205,
8206,
8202,
8197,
8193,
8192,
8192,
8195,
8195,
8193,
8195,
8201,
8205,
8206,
8205,
8206,
8206,
8202,
8202,
8207,
8214,
8216,
8209,
8203,
8199,
8198,
8196,
8195,
8196,
8202,
8204,
8204,
8200,
8196,
8196,
8195,
8195,
8196,
8195,
8194,
8198,
8207,
8210,
8205,
8202,
8201,
8202,
8203,
8200,
8198,
8199,
8200,
8202,
8202,
8202,
8203,
8202,
8205,
8204,
8197,
8196,
8201,
8209,
8221,
8233,
8242,
8244,
8241,
8239,
8246,
8258,
8269,
8277,
8282,
8277,
8268,
8261,
8256,
8243,
8223,
8200,
8182,
8173,
8170,
8166,
8165,
8164,
8163,
8159,
8155,
8155,
8155,
8152,
8151,
8149,
8146,
8148,
8156,
8156,
8151,
8147,
8147,
8151,
8155,
8157,
8161,
8161,
8161,
8161,
8167,
8172,
8175,
8179,
8184,
8186,
8184,
8181,
8179,
8174,
8175,
8192,
8227,
8270,
8333,
8434,
8577,
8732,
8841,
8811,
8619,
8379,
8193,
8090,
8058,
8063,
8075,
8097,
8134,
8180,
8220,
8244,
8254,
8262,
8268,
8269,
8269,
8270,
8272,
8276,
8278,
8279,
8280,
8282,
8286,
8288,
8290,
8289,
8290,
8291,
8290,
8289,
8288,
8284,
8280,
8282,
8290,
8295,
8297,
8300,
8307,
8313,
8320,
8327,
8334,
8338,
8337,
8336,
8339,
8347,
8359,
8367,
8369,
8370,
8374,
8382,
8394,
8406,
8416,
8426,
8444,
8465,
8479,
8488,
8504,
8524,
8544,
8559,
8574,
8587,
8596,
8608,
8624,
8636,
8641,
8641,
8641,
8636,
8626,
8610,
8596,
8580,
8558,
8529,
8499,
8469,
8436,
8401,
8371,
8348,
8328,
8306,
8283,
8264,
8249,
8232,
8212,
8200,
8193,
8190,
8189,
8187,
8182,
8177,
8177,
8180,
8185,
8189,
8192,
8193,
8193,
8189,
8183,
8181,
8186,
8192,
8195,
8194,
8193,
8192,
8192,
8192,
8196,
8198,
8204,
8207,
8207,
8206,
8205,
8203,
8201,
8199,
8200,
8200,
8197,
8197,
8200,
8200,
8195,
8192,
8192,
8193,
8191,
8186,
8179,
8174,
8174,
8177,
8178,
8177,
8174,
8170,
8167,
8169,
8175,
8175,
8172,
8173,
8175,
8177,
8176,
8175,
8177,
8175,
8170,
8165,
8160,
8157,
8159,
8162,
8167,
8167,
8167,
8168,
8171,
8172,
8172,
8169,
8170,
8169,
8172,
8175,
8177,
8178,
8178,
8176,
8175,
8175,
8179,
8177,
8170,
8165,
8164,
8171,
8179,
8184,
8187,
8187,
8183,
8180,
8180,
8177,
8176,
8176,
8179,
8184,
8187,
8186,
8184,
8186,
8190,
8191,
8190,
8188,
8187,
8186,
8186,
8187,
8188,
8188,
8189,
8189,
8186,
8182,
8179,
8178,
8179,
8180,
8185,
8191,
8193,
8195,
8198,
8197,
8195,
8195,
8199,
8200,
8194,
8188,
8188,
8189,
8189,
8190,
8192,
8190,
8187,
8185,
8186,
8193,
8198,
8195,
8194,
8192,
8189,
8190,
8193,
8197,
8201,
8200,
8197,
8194,
8196,
8199,
8197,
8196,
8197,
8196,
8196,
8196,
8193,
8192,
8194,
8192,
8191,
8191,
8196,
8200,
8200,
8201,
8201,
8198,
8193,
8189,
8184,
8181,
8181,
8181,
8180,
8179,
8179,
8181,
8183,
8188,
8193,
8196,
8198,
8198,
8198,
8196,
8192,
8196,
8204,
8211,
8216,
8221,
8228,
8235,
8242,
8249,
8257,
8262,
8266,
8267,
8266,
8259,
8250,
8242,
8236,
8227,
8214,
8197,
8185,
8177,
8174,
8173,
8172,
8166,
8158,
8156,
8163,
8171,
8177,
8176,
8174,
8169,
8166,
8166,
8170,
8167,
8163,
8164,
8169,
8173,
8174,
8174,
8174,
8175,
8176,
8176,
8175,
8173,
8170,
8171,
8174,
8175,
8175,
8179,
8193,
8221,
8251,
8291,
8377,
8512,
8667,
8801,
8844,
8725,
8492,
8273,
8132,
8074,
8068,
8071,
8078,
8101,
8142,
8190,
8223,
8241,
8252,
8260,
8266,
8269,
8269,
8268,
8269,
8270,
8270,
8271,
8273,
8276,
8279,
8280,
8284,
8284,
8284,
8282,
8280,
8282,
8287,
8290,
8297,
8303,
8306,
8305,
8303,
8304,
8308,
8308,
8307,
8310,
8318,
8327,
8333,
8335,
8337,
8341,
8349,
8356,
8364,
8368,
8371,
8381,
8397,
8415,
8429,
8444,
8460,
8474,
8486,
8498,
8514,
8533,
8551,
8567,
8583,
8597,
8610,
8621,
8631,
8634,
8631,
8630,
8629,
8624,
8612,
8591,
8568,
8543,
8515,
8488,
8455,
8420,
8390,
8364,
8344,
8325,
8302,
8279,
8260,
8246,
8234,
8220,
8206,
8195,
8188,
8180,
8174,
8167,
8165,
8169,
8175,
8177,
8176,
8173,
8172,
8177,
8182,
8183,
8181,
8177,
8173,
8173,
8174,
8175,
8178,
8180,
8179,
8177,
8179,
8181,
8184,
8184,
8183,
8185,
8190,
8191,
8190,
8187,
8185,
8184,
8182,
8177,
8171,
8164,
8159,
8159,
8158,
8156,
8157,
8163,
8171,
8176,
8175,
8168,
8160,
8154,
8149,
8148,
8150,
8154,
8158,
8158,
8156,
8156,
8157,
8154,
8150,
8147,
8146,
8145,
8145,
8148,
8150,
8148,
8146,
8143,
8142,
8142,
8146,
8150,
8153,
8153,
8150,
8145,
8143,
8147,
8154,
8161,
8164,
8162,
8161,
8159,
8157,
8158,
8161,
8161,
8161,
8160,
8163,
8164,
8164,
8163,
8165,
8167,
8170,
8172,
8173,
8171,
8168,
8166,
8168,
8170,
8170,
8167,
8166,
8166,
8164,
8162,
8161,
8165,
8171,
8178,
8184,
8186,
8184,
8181,
8180,
8179,
8178,
8182,
8186,
8186,
8186,
8181,
8174,
8172,
8176,
8184,
8191,
8193,
8193,
8190,
8184,
8178,
8173,
8174,
8176,
8177,
8176,
8174,
8174,
8176,
8182,
8190,
8193,
8190,
8185,
8185,
8187,
8189,
8192,
8192,
8189,
8187,
8185,
8185,
8187,
8183,
8177,
8178,
8184,
8187,
8187,
8186,
8186,
8187,
8188,
8188,
8190,
8190,
8188,
8188,
8193,
8202,
8210,
8214,
8219,
8221,
8220,
8219,
8223,
8232,
8239,
8244,
8250,
8251,
8246,
8237,
8229,
8222,
8214,
8203,
8192,
8179,
8167,
8160,
8160,
8161,
8162,
8160,
8155,
8149,
8147,
8149,
8154,
8157,
8156,
8152,
8152,
8159,
8165,
8168,
8166,
8163,
8159,
8157,
8158,
8156,
8156,
8163,
8173,
8181,
8182,
8181,
8181,
8180,
8180,
8180,
8178,
8178,
8176,
8174,
8189,
8220,
8260,
8318,
8414,
8548,
8693,
8807,
8808,
8645,
8410,
8214,
8099,
8057,
8055,
8063,
8080,
8114,
8162,
8204,
8232,
8245,
8251,
8254,
8254,
8253,
8252,
8255,
8260,
8260,
8257,
8256,
8259,
8264,
8269,
8274,
8279,
8279,
8278,
8278,
8281,
8284,
8284,
8284,
8283,
8286,
8290,
8289,
8292,
8295,
8298,
8299,
8305,
8312,
8315,
8321,
8330,
8331,
8329,
8331,
8341,
8353,
8364,
8371,
8377,
8381,
8390,
8402,
8412,
8420,
8430,
8444,
8463,
8479,
8492,
8504,
8520,
8539,
8556,
8571,
8588,
8600,
8607,
8611,
8615,
8618,
8616,
8611,
8600,
8584,
8561,
8524,
8493,
8469,
8441,
8405,
8372,
8346,
8322,
8297,
8268,
8247,
8235,
8226,
8216,
8203,
8192,
8182,
8178,
8175,
8170,
8167,
8165,
8159,
8152,
8149,
8151,
8154,
8156,
8155,
8153,
8153,
8159,
8166,
8169,
8166,
8161,
8160,
8162,
8167,
8171,
8173,
8177,
8177,
8174,
8172,
8173,
8173,
8168,
8165,
8167,
8172,
8177,
8178,
8179,
8175,
8170,
8164,
8161,
8162,
8167,
8167,
8164,
8161,
8162,
8162,
8161,
8159,
8154,
8150,
8148,
8148,
8150,
8150,
8150,
8150,
8152,
8151,
8150,
8149,
8149,
8150,
8156,
8154,
8147,
8147,
8149,
8151,
8153,
8151,
8147,
8150,
8154,
8156,
8156,
8155,
8154,
8152,
8150,
8153,
8161,
8168,
8172,
8169,
8166,
8170,
8173,
8171,
8169,
8170,
8169,
8165,
8162,
8160,
8160,
8161,
8163,
8167,
8173,
8178,
8180,
8179,
8180,
8178,
8175,
8175,
8180,
8187,
8191,
8192,
8191,
8190,
8191,
8189,
8186,
8180,
8176,
8177,
8184,
8185,
8185,
8187,
8190,
8192,
8192,
8191,
8187,
8185,
8188,
8187,
8183,
8181,
8183,
8184,
8186,
8189,
8192,
8191,
8186,
8182,
8187,
8193,
8198,
8199,
8198,
8196,
8195,
8195,
8196,
8194,
8189,
8187,
8191,
8192,
8193,
8195,
8198,
8198,
8196,
8195,
8195,
8194,
8192,
8190,
8189,
8188,
8189,
8194,
8201,
8201,
8197,
8194,
8193,
8192,
8192,
8191,
8188,
8190,
8194,
8195,
8196,
8194,
8194,
8194,
8195,
8198,
8199,
8193,
8186,
8182,
8183,
8187,
8194,
8198,
8197,
8196,
8196,
8197,
8198,
8200,
8207,
8212,
8216,
8224,
8232,
8233,
8230,
8230,
8236,
8248,
8259,
8261,
8260,
8259,
8256,
8250,
8241,
8232,
8221,
8204,
8187,
8174,
8167,
8168,
8171,
8174,
8175,
8174,
8175,
8175,
8175,
8173,
8172,
8170,
8170,
8170,
8171,
8168,
8163,
8163,
8170,
8171,
8169,
8166,
8167,
8173,
8180,
8183,
8184,
8186,
8188,
8187,
8186,
8188,
8194,
8197,
8191,
8181,
8183,
8197,
8227,
8265,
8333,
8447,
8591,
8740,
8843,
8804,
8604,
8362,
8181,
8084,
8066,
8076,
8084,
8096,
8125,
8171,
8214,
8238,
8245,
8247,
8255,
8264,
8265,
8261,
8258,
8262,
8270,
8280,
8285,
8280,
8274,
8272,
8273,
8275,
8278,
8281,
8284,
8287,
8292,
8296,
8300,
8301,
8300,
8297,
8294,
8297,
8302,
8306,
8311,
8316,
8323,
8328,
8332,
8336,
8341,
8347,
8354,
8358,
8361,
8366,
8376,
8390,
8405,
8413,
8418,
8428,
8439,
8452,
8470,
8488,
8503,
8515,
8530,
8546,
8564,
8581,
8598,
8614,
8626,
8636,
8644,
8650,
8653,
8648,
8638,
8619,
8596,
8572,
8549,
8521,
8493,
8467,
8439,
8408,
8376,
8348,
8325,
8303,
8283,
8261,
8243,
8232,
8226,
8219,
8211,
8204,
8200,
8197,
8195,
8191,
8185,
8183,
8187,
8192,
8192,
8189,
8184,
8178,
8175,
8179,
8192,
8202,
8204,
8203,
8202,
8202,
8202,
8202,
8201,
8203,
8205,
8206,
8204,
8201,
8200,
8199,
8198,
8197,
8196,
8195,
8194,
8193,
8192,
8191,
8191,
8190,
8189,
8186,
8185,
8186,
8186,
8184,
8182,
8178,
8175,
8173,
8170,
8169,
8170,
8168,
8166,
8164,
8163,
8163,
8162,
8160,
8158,
8159,
8162,
8162,
8159,
8158,
8164,
8174,
8181,
8182,
8179,
8176,
8174,
8171,
8168,
8167,
8170,
8170,
8169,
8171,
8174,
8170,
8163,
8158,
8160,
8165,
8170,
8171,
8172,
8172,
8173,
8175,
8179,
8177,
8174,
8173,
8175,
8177,
8179,
8180,
8180,
8180,
8182,
8182,
8182,
8181,
8181,
8177,
8173,
8172,
8172,
8174,
8180,
8185,
8187,
8186,
8184,
8184,
8189,
8192,
8192,
8192,
8195,
8193,
8189,
8187,
8191,
8192,
8192,
8191,
8192,
8192,
8191,
8189,
8188,
8187,
8185,
8179,
8174,
8175,
8182,
8187,
8192,
8192,
8188,
8186,
8188,
8191,
8188,
8185,
8182,
8176,
8171,
8172,
8179,
8187,
8192,
8192,
8192,
8192,
8194,
8192,
8190,
8188,
8188,
8186,
8184,
8185,
8191,
8192,
8192,
8192,
8194,
8193,
8189,
8189,
8192,
8195,
8194,
8197,
8206,
8218,
8225,
8227,
8222,
8218,
8220,
8222,
8223,
8229,
8243,
8248,
8242,
8230,
8221,
8216,
8211,
8203,
8192,

        };
        var bytes = data.SelectMany(BitConverter.GetBytes).ToArray();
        var dataString = Convert.ToBase64String(bytes);
        Console.WriteLine(dataString);
    }
}