SeaMist a .NET library for the Kraken.io REST API
=============
[![Build status](https://ci.appveyor.com/api/projects/status/y166lwaub0596dk8?svg=true)](https://ci.appveyor.com/project/Kevin-Bronsdijk/SeaMist) [![NuGet](https://img.shields.io/nuget/v/SeaMist.svg?maxAge=2592000)](https://www.nuget.org/packages/SeaMist/)

***
### SeaMist has evolved to [kraken-net](https://github.com/Kevin-Bronsdijk/kraken-net), the official Kraken.io .Net client.
 With the introduction of kraken-net, the development on SeaMist has been ceased. Therefore, it’s highly recommend moving to kraken-net. 
 
 The upgrade is simple and the required steps can be found within the [upgrade guide](https://github.com/Kevin-Bronsdijk/kraken-net#upgrade-from-seamist).
***
https://github.com/Kevin-Bronsdijk/kraken-net/

The SeaMist library interacts with the Kraken.io REST API allowing you to utilize Kraken’s features using a .NET interface. 

* [Getting Started](#getting-started)
* [Downloading Images](#downloading-images)
* [How To Use](#how-to-use)
* [Authentication](#authentication)
* [Direct upload](#direct-upload)
* [Wait and Callback URL](#wait-and-callback-url)
  * [Wait Option](#wait-option)
  * [Callback URL](#callback-url)
* [External Storage](#external-storage)
  * [Azure Blob](#azure-blob)
  * [Amazon S3](#amazon-s3)
* [Lossy Optimization](#lossy-optimization)
* [WebP Compression](#webp-compression)
* [Preserving Metadata](#preserving-metadata)
* [Automatic Image Orientation](#automatic-image-orientation)
* [Chroma Subsampling](#chroma-subsampling)
* [Image Resizing](#image-resizing)
* [Image Type Conversion](#image-type-conversion)
* [API Sandbox](#api-sandbox)
* [Reseller Account](#reseller-account)
* [Account Status](#account-status)


## Getting Started

First you need to sign up for the [Kraken API](http://kraken.io/plans/) and obtain your unique **API Key** and **API Secret**. You will find both under [API Credentials](http://kraken.io/account/api-credentials). Once you have set up your account, you can start using SeaMist.

## Downloading Images

Remember - never link to optimized images offered to download. You have to download them first, and then replace them in your websites or applications. Due to security reasons optimized images are available on our servers **for one hour** only.

## How to use

You can optimize your images by providing the URL of the image you want to optimize or upload the image directly instead. Just keep in mind that the image URL must be accessible for Kraken.

## Authentication

The first step is to authenticate to Kraken API by providing your unique API Key and API Secret while creating a new SeaMist connection.

```C#
 var connection = KrakenConnection.Create("key", "secret");
```

## Direct upload
SeaMist for Kraken allows you to easily upload your images as can be seen within the examples below. Use the full path option or provide a byte array and name.

**Wait using a local file path:** 

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.OptimizeWait("c:\your-image-location-on-disk.png",
    new OptimizeUploadWaitRequest()
    {
       // your compression settings
    }
);
```

**Wait using a byte array:**

```C#
var krakenClient = new KrakenClient(connection);

var image = File.ReadAllBytes("c:\your-image-location-on-disk.png"); 

var response = await krakenClient.OptimizeWait(
    image,
    "name-of-image.png",
    new OptimizeUploadWaitRequest()
    {
        // your compression settings
    }
);
```

**Callback using a local file path:** 

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.Optimize("c:\your-image-location-on-disk.png",
    new OptimizeUploadRequest(new Uri("http://awesome-website.com/kraken_results"))
    {
        // your compression settings
    }
);
```

**Callback Url using a byte array:**

```C#
var krakenClient = new KrakenClient(connection);

var image = File.ReadAllBytes("c:\your-image-location-on-disk.png"); 

var response = await krakenClient.Optimize(
    image,
    "name-of-image.png",
    new OptimizeUploadRequest(new Uri("http://awesome-website.com/kraken_results"))
    {
        // your compression settings
    }
);
```

## Wait and Callback URL

SeaMist gives you two options for fetching optimization results. Using the `OptimizeWait` request the results will be returned in the response. Using the `Optimize` request the results will be posted to the URL specified in your request.

### Wait option

Using the `OptimizeWait` request, the connection will be held open until the image has been optimized. Once this is done you will recieve a `OptimizeWaitResult` object containing your optimization results. 

**Request:**

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.OptimizeWait(
    new Uri("http://image-url.com/file.jpg")
);
```

### Callback URL

Using the `OptimizeWait` request the connection will be terminated immediately and a unique `id` will be returned in the `OptimizeWaitResult` object. After the optimization is over Kraken will POST a message to the `callbackUrl` specified in your request. The ID in the response will reflect the ID in the results posted to your Callback URL.

**Request:**

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.Optimize(
    new Uri("http://image-url.com/file.jpg"),
    new Uri("http://awesome-website.com/kraken_results")
);
```

**Results posted to the Callback URL:**

````js
{
    "id": "18fede37617a787649c3f60b9f1f280d"
    "success": true,
    "file_name": "file.jpg",
    "original_size": 324520,
    "kraked_size": 165358,
    "saved_bytes": 159162,
    "kraked_url": "http://dl.kraken.io/18fede37617a787649c3f60b9f1f280d/file.jpg"
}
````

## External Storage
SeaMist supports the option which allows you to store optimized images directly in your Microsoft Azure Blob Storage or S3 bucket. With just a few additional parameters your optimized images will be pushed to Microsoft Azure or S3.

### Azure Blob

**Azure Blob Storage:**

```C#
using SeaMist.Model.Azure;

var krakenClient = new KrakenClient(connection);

var response = await krakenClient.OptimizeWait(
   new Uri("http://image-url.com/file.jpg"),
   new DataStore(
      "account",
      "key ",
      "container"
      )
);
```

**Azure Blob Storage with options - Deprecated:**

```C#
var response = await krakenClient.OptimizeWait(new Model.Azure.OptimizeWaitRequest()
{
   ImageUrl = new Uri("http://image-url.com/file.jpg"),
   Lossy = true,
   BlobStore = new DataStore(
       "account",
       "key",
       "container")
});
```

**Azure Blob Storage with options:**

```C#
var response = await krakenClient.OptimizeWait(new Model.Azure.OptimizeWaitRequest(
        new Uri("http://image-url.com/file.jpg"), "account", "key", "container")
        {
            ResizeImage = new ResizeImage { Height = 100, Width = 100 },
            WebP = true
        }
);
```

**Azure Blob Storage upload with options:**

```C#
var image = File.ReadAllBytes("your-image-location-on-disk.png"); 

var response = await krakenClient.OptimizeWait(
    image, 
    "image-name.jpg",
    new Model.Azure.OptimizeUploadWaitRequest("account", "key","container")
    {
        ResizeImage = new ResizeImage { Height = 100, Width = 100 },
        WebP = true
    });
```

**Azure Blob Storage with custom headers and metadata:**

```C#
var dataStore = new SeaMist.Model.Azure.DataStore("account", "key","container");

dataStore.AddMetadata("x-ms-meta-test1", "value1"); 
dataStore.AddHeaders("Cache-Control", "max-age=2222");

var response = krakenClient.OptimizeWait(
    new SeaMist.Model.Azure.OptimizeWaitRequest(new Uri(TestData.ImageOne), dataStore)
    {
        WebP = true
    }
);
```


### Amazon S3

**Amazon S3 - Deprecated:**

```C#
using SeaMist.Model.S3;

var krakenClient = new KrakenClient(connection);

var response = await krakenClient.OptimizeWait(
   new Uri("http://image-url.com/file.jpg"),
   new DataStore(
      "key",
      "secret",
      "bucket",
      "region"
      )
);

```

**Amazon S3:**

```C#
using SeaMist.Model.S3;

var krakenClient = new KrakenClient(connection);

 var response = await krakenClient.OptimizeWait(new Model.S3.OptimizeWaitRequest(
    new Uri("http://image-url.com/file.jpg"), "account", "key", "container", "region")
    {
        ResizeImage = new ResizeImage { Height = 100, Width = 100 },
        WebP = true
    });
    
```

**Amazon S3 upload with options:**

```C#
var image = File.ReadAllBytes("your-image-location-on-disk.png"); 

var response = await krakenClient.OptimizeWait(
    image, 
    "image-name.jpg",
    new Model.S3.OptimizeUploadWaitRequest("account", "key", "container", "region")
    {
        ResizeImage = new ResizeImage {Height = 100, Width = 100},
        WebP = true
    });
```

**Azure Blob Storage with custom headers and metadata:**

```C#
var dataStore = new SeaMist.Model.S3.DataStore("account", "key","container");

dataStore.AddMetadata("x-amz-meta-test1", "value1"); 
dataStore.AddHeaders("Cache-Control", "public, max-age=123456");

var response = krakenClient.OptimizeWait(
    new SeaMist.Model.S3.OptimizeWaitRequest(new Uri(TestData.ImageOne), dataStore)
    {
        WebP = true
    }
);
```

## Lossy Optimization

When you decide to sacrifice just a small amount of image quality (usually unnoticeable to the human eye), you will be able to save up to 90% of the initial file weight. Lossy optimization will give you outstanding results with just a fraction of image quality loss.

To use lossy optimizations simply set `Lossy = true` in your request:

```C#
var krakenClient = new KrakenClient(connection);

var request = new OptimizeWaitRequest(
    new Uri("http://image-url.com/file.jpg"))
    {
        Lossy = true,
    };

var response = await krakenClient.OptimizeWait(request);

```

### PNG Images
PNG images will be converted from 24-bit to paletted 8-bit with full alpha channel. This process is called PNG quantization in RGBA format and means the amount of colours used in an image will be reduced to 256 while maintaining all information about alpha transparency.

### JPEG Images
For lossy JPEG optimizations Kraken will generate multiple copies of a input image with a different quality settings. It will then intelligently pick the one with the best quality to filesize ration. This ensures your JPEG image will be at the smallest size with the highest possible quality, without the need for a human to select the optimal image.

## WebP Compression

WebP is a new image format introduced by Google in 2010 which supports both lossy and lossless compression. According to [Google](https://developers.google.com/speed/webp/), WebP lossless images are **26% smaller** in size compared to PNGs and WebP lossy images are **25-34% smaller** in size compared to JPEG images.

To recompress your PNG or JPEG files into WebP format simply set `WebP = true` flag in your optimize request. You can also optionally set `Lossy = true` flag to leverage WebP's lossy compression:

```C#
var krakenClient = new KrakenClient(connection);

var request = new OptimizeWaitRequest(
    new Uri("http://image-url.com/file.jpg"))
    {
        Lossy = true,
        WebP = true
    };

var response = await krakenClient.OptimizeWait(request);
```

## Preserving Metadata

By default Kraken API will strip all the metadata found in an image to make the image file as small as it is possible, and in both lossy and lossless modes. Entries like EXIF, XMP and IPTC tags, colour profile information, etc. will be stripped altogether.

However there are situations when you might want to preserve some of the meta information contained in the image, for example, copyright notice or geotags. In order to preserve the most important meta entries add an additional PreserveMeta array to your request with one or more of the following values:
`Date` `Copyright` `Geotag` `Orientation` `Profile`

```C#
var krakenClient = new KrakenClient(connection);

var request = new OptimizeWaitRequest(
    new Uri("http://image-url.com/file.jpg"))
    {
       PreserveMeta = new PreserveMeta[] { PreserveMeta.Geotag }
    };

var response = await krakenClient.OptimizeWait(request);
```
## Automatic Image Orientation

The EXIF (exchangeable image file format) standard specifies an Orientation tag that can be embedded in images, and is usually set in accordance with the reading from a gravity sensor or accelerometer in digital cameras and smartphones. This enables you to take a picture with your camera sideways or upside-down, and stand a reasonable chance of having it display properly on your computer.

For more information on Automatic Image Orientation, please consult the kraken documentation. 

Code sample:

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.OptimizeWait("c:\your-image-location-on-disk.jpg",
    new OptimizeUploadWaitRequest()
    {
        AutoOrient = true
    }
);
```

## Chroma Subsampling 

JPEG is a lossy compression algorithm, meaning that it trades quality to achieve a smaller file size.

The whole point of the JPEG compression format is to reproduce photographs so as to minimize file size while keeping the visual qualities as accurate to the original as possible. For more information on chroma subsampling, please consult the kraken documentation. 

The following options are supported: `Default` (4:2:0) `S422` (4:2:0) or `S444` (4:4:4).

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.OptimizeWait("c:\your-image-location-on-disk.jpg",
    new OptimizeUploadWaitRequest()
    {
        Lossy = true,
        WebP = true,
        SamplingScheme.S422
    }
);
```

## Image Resizing

Image resizing option is great for creating thumbnails or preview images in your applications. Kraken will first resize the given image and then optimize it with its vast array of optimization algorithms. The `resize` option needs a few parameters to be passed like desired `width` and/or `height` and a mandatory `strategy` property. For example:

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.OptimizeWait("c:\your-image-location-on-disk.jpg",
    new OptimizeUploadWaitRequest()
    {
        ResizeImage = new ResizeImage
        {
            Width  = 100,
            Height = 100,
            Strategy = Strategy.fit
        }
    }
);
```

The `strategy` property can have one of the following values:

- `exact` - Resize by exact width/height. No aspect ratio will be maintained.
- `portrait` - Exact width will be set, height will be adjusted according to aspect ratio.
- `landscape` - Exact height will be set, width will be adjusted according to aspect ratio.
- `auto` - The best strategy (portrait or landscape) will be selected for a given image according to aspect ratio.
- `fit`  - This option will crop and resize your images to fit the desired width and height
- `crop` - This option will crop your image to the exact size you specify with no distortion.
- `square` - This strategy will first crop the image by its shorter dimension to make it a square, then resize it to the specified size.
- `fill` - This strategy allows you to resize the image to fit the specified bounds while preserving the aspect ratio (just like auto strategy). The optional background property allows you to specify a color which will be used to fill the unused portions of the previously specified bounds.
The background property can be formatted in HEX notation "#f60" or "#ff6600", RGB "rgb(255, 0, 0)" or RGBA "rgba(91, 126, 156, 0.7)". The default background color is white. Example usage of fill strategy:

## Image Type Conversion

Kraken allows you to easily convert different images from one type/format to another. If, for example, you would like to turn you transparent PNG file into a JPEG with a grey background.

In order to convert between different image types you need to add the `ConvertImage` object to you request. This object takes two properties:

`format` —	The image format you wish to convert your image into. This can accept one of the following values: `ImageFormat.jpeg`, `ImageFormat.png` or `ImageFormat.gif`.

`background` —	Background image when converting from transparent file formats like PNG or GIF into fully opaque format like JPEG. The background property can be passed in HEX notation "#f60" or "#ff6600". The default background color is white.

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.OptimizeWait("c:\your-image-location-on-disk.jpg",
    new OptimizeUploadWaitRequest()
    {
        ConvertImage = new ConvertImage(ImageFormat.gif)
        {
            BackgroundColor = "#ffffff"
        }
    }
);
```

## API Sandbox

The Kraken.io API Sandbox is designed to allow you to take all the time you need to integrate with the Kraken.io. Kraken.io endpoints will parse your request, validate your JSON, process uploads, etc. Everything will reflect production usage on a fully-featured Kraken API plan apart from the actual optimization process itself. Instead Kraken API will return randomized optimization results. 

```C#
 var connection = KrakenConnection.Create("key", "secret", true);
```

## Reseller Account

Kraken.io Premium and Enterprise plans are Reseller enabled. If you would like to be a reseller, or provision your customers with Kraken API credentials which you can control the price of, our Reseller Account is perfect for you.

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.ResellerAccount();
```
## Account Status

Kraken.io allows you to programatically query your account status, enabling you to retrieve details such as the name of the plan you have subscribed to, your total quota, used quota, remaining quota, and the "active" status of your account.

```C#
var krakenClient = new KrakenClient(connection);

var response = await krakenClient.UserStatus();
```

## LICENSE - MIT

Copyright (c) 2016 Kevin Bronsdijk - http://devslice.net/

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
