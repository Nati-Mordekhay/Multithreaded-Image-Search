# Multithreaded-Image-Search

C# console application locating nested images within a larger image using multithreading, locating nested images using either exact or euclidean match.

App inputs
----------
* args[0]: 'image1' the names of the full image
* args[1]: 'image2' the name of the nested image 
* args[2]s: the number of threads to find the nested image (1 or greater)
* args[3]: find the nested image using either 'exact' match or 'euclidean' match

Accepted image formats: jpg, gif, png

Running example: 'imageSearch.exe bigImage.png nestedImage.png 10 exact'

![Image2](https://github.com/Nati-Mordekhay/Multithreaded-Image-Search/assets/72460220/627a754e-6fa9-4e83-86fd-bf4e9be016ae)
![Image1 (1)](https://github.com/Nati-Mordekhay/Multithreaded-Image-Search/assets/72460220/404c453c-7705-495c-bbde-a3516e87f4ed)
![example](https://github.com/Nati-Mordekhay/Multithreaded-Image-Search/assets/72460220/bdb4fbce-5bb1-4200-a2c8-6bd782d8a11f)
