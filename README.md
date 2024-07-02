# Multithreaded-Image-Search

C# console application locating nested images within a larger image using multithreading, locating nested images using either exact or euclidean match.

App inputs
----------
* args[0]: 'image1' the names of the full image
* args[1]: 'image2' the name of the nested image 
* args[2]s: the number of threads to find the nested image (1 or greater)
* args[3]: find the nested image using either 'exact' match or 'euclidean' match

Accepted image formats: jpg, gif, png

Running example: 'imageSearch.exe imageBig.png imageSmall.png 4 exact', 'imageSearch.exe imageBig.png imageSmall.png 6 euclidian':

![exampleRun](https://github.com/Nati-Mordekhay/Multithreaded-Image-Search/assets/72460220/74b376f7-060a-4f7d-9d5c-e06cbb88b074)
