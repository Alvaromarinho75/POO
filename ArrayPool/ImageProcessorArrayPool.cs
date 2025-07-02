using System;
using System.Buffers;
using System.Diagnostics;

public class ImageProcessorArrayPool
{
    private const int IMAGE_WIDTH = 800;
    private const int IMAGE_HEIGHT = 600;
    private const int TOTAL_IMAGES = 500;

    public static void ProcessImages()
    {
        Console.WriteLine("Iniciando processamento de imagens (ArrayPool)...");
        var stopwatch = Stopwatch.StartNew();
        int processedCount = 0;
        int totalPixels = IMAGE_WIDTH * IMAGE_HEIGHT;
        for (int imageIndex = 0; imageIndex < TOTAL_IMAGES; imageIndex++)
        {
            var pool = ArrayPool<PixelRGB>.Shared;
            PixelRGB[] originalImage = pool.Rent(totalPixels);
            PixelRGB[] blurredImage = pool.Rent(totalPixels);
            try
            {
                GenerateSyntheticImage(imageIndex, originalImage);
                ApplyBlurFilter(originalImage, blurredImage);
                SaveImage(blurredImage, $"processed_{imageIndex}.jpg");
                processedCount++;
                if (imageIndex % 50 == 0)
                {
                    Console.WriteLine($"Processadas {imageIndex} imagens...");
                }
            }
            finally
            {
                pool.Return(originalImage, clearArray: false);
                pool.Return(blurredImage, clearArray: false);
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"Processamento concluído!");
        Console.WriteLine($"Imagens processadas: {processedCount}");
        Console.WriteLine($"Tempo total: {stopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine($"Tempo médio por imagem: {stopwatch.ElapsedMilliseconds / (double)processedCount:F2} ms");
    }

    private static void GenerateSyntheticImage(int seed, PixelRGB[] image)
    {
        var random = new Random(seed);
        for (int y = 0; y < IMAGE_HEIGHT; y++)
        {
            for (int x = 0; x < IMAGE_WIDTH; x++)
            {
                image[y * IMAGE_WIDTH + x] = new PixelRGB(
                    (byte)random.Next(256),
                    (byte)random.Next(256),
                    (byte)random.Next(256)
                );
            }
        }
    }

    private static void ApplyBlurFilter(PixelRGB[] original, PixelRGB[] blurred)
    {
        for (int y = 0; y < IMAGE_HEIGHT - 1; y++)
        {
            for (int x = 0; x < IMAGE_WIDTH - 1; x++)
            {
                int idx = y * IMAGE_WIDTH + x;
                blurred[idx] = PixelRGB.Average(
                    original[idx],
                    original[idx + 1],
                    original[idx + IMAGE_WIDTH],
                    original[idx + IMAGE_WIDTH + 1]
                );
            }
        }
    }

    private static void SaveImage(PixelRGB[] image, string filename)
    {
        // Simula salvamento
    }
} 