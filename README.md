SeaMist a .NET library for the Kraken.io REST API
=============

The SeaMist library interacts with the Kraken.io REST API allowing you to utilize Kraken’s features using a .NET interface. Latest [blog post](http://devslice.net/2016/02/seamist-update-external-storage/) covering SeaMist updates.

* [Getting Started](#getting-started)
* [Downloading Images](#downloading-images)
* [How To Use](#how-to-use)
* [Authentication](#authentication)
* [Wait and Callback URL](#wait-and-callback-url)
  * [Wait Option](#wait-option)
  * [Callback URL](#callback-url)
* [External Storage](#external-storage)
  * [Azure Blob](#azure-blob)
  * [Amazon S3](#amazon-s3)
* [Lossy Optimization](#lossy-optimization)
* [WebP Compression](#webp-compression)
* [Image Resizing](#image-resizing)

## Getting Started

First you need to sign up for the [Kraken API](http://kraken.io/plans/) and obtain your unique **API Key** and **API Secret**. You will find both under [API Credentials](http://kraken.io/account/api-credentials). Once you have set up your account, you can start using SeaMist.

## Downloading Images

Remember - never link to optimized images offered to download. You have to download them first, and then replace them in your websites or applications. Due to security reasons optimized images are available on our servers **for one hour** only.

## How to use

You can optimize your images by providing the URL of the image you want to optimize. Just keep in mind that the image URL must be accessible for Kraken. The upload option isn’t supported within the current version of SeaMist.

## Authentication

The first step is to authenticate to Kraken API by providing your unique API Key and API Secret while creating a new SeaMist connection.

```C#
 var connection = KrakenConnection.Create("key", "secret");
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

**Request Azure Blob Storage:**

```C#
using SeaMist.Model.Azure;

var krakenClient = new KrakenClient(connection);

response = await client.OptimizeWait(
   new Uri("http://image-url.com/file.jpg"),
   new DataStore(
      "account",
      "key ",
      "container"
      )
);

// Azure using OptimizeWaitRequest. Enables all options.
response = await client.OptimizeWait(new SeaMist.Model.Azure.OptimizeWaitRequest()
{
   ImageUrl = new Uri("http://image-url.com/file.jpg"),
      Lossy = true,
      BlobStore = new DataStore(
        "account",
        "key",
        "container"
        )
}
);

```
### Amazon S3

**Request Amazon S3 bucket:**

```C#
using SeaMist.Model.S3;

var krakenClient = new KrakenClient(connection);

response = await client.OptimizeWait(
   new Uri("http://image-url.com/file.jpg"),
   new DataStore(
      "key",
      "secret",
      "bucket",
      "region"
      )
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

var request = new OptimizeRequest(
    new Uri("http://image-url.com/file.jpg"))
    {
        Lossy = true,
        WebP = true
    };

var response = await krakenClient.OptimizeWait(request);
```

## Image Resizing

Image resizing option is great for creating thumbnails or preview images in your applications. Kraken will first resize the given image and then optimize it with its vast array of optimization algorithms. The `resize` option needs a few parameters to be passed like desired `width` and/or `height` and a mandatory `strategy` property. For example:

```C#
var krakenClient = new KrakenClient(connection);

var optimizeRequest = new OptimizeWaitRequest(
    new Uri("http://image-url.com/file.jpg"))
    {
        ResizeImage = new ResizeImage
        {
            Width  = 100,
            Height = 100,
            Strategy = Strategy.fit
        }
    };

var response = await krakenClient.OptimizeWait(request);
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
