# PFFT
Showing ICMP TTL data in a Fast Fourier Transformer (POC/WIP)

FFT of ICMP latency:

- Identifying periodic issues: If there are any periodic issues affecting latency, they will show up as peaks at corresponding frequencies in the FFT. For example, if there's a process that runs every 10 seconds and slows down the network, you would see a peak at the 0.1 Hz frequency.

- Identifying persistent problems: If the latency is consistently high, this will appear as a peak at the 0Hz frequency (often called the DC component in FFT results). If you see a high DC component, it could be an indication of a persistent problem with latency.

- Identifying random issues: If there are a lot of random, non-periodic issues affecting latency, these will show up as a wide, flat spectrum in the FFT. If you see a spectrum that doesn't have distinct peaks but is high across a wide range of frequencies, it could mean that there's a lot of random jitter in the latency.


Understanding the nature of latency: By looking at the spectrum, you can get a better understanding of whether latency issues are steady, periodic, or random.

Quick run ICMP Spectrum (Samplerate: 512)
![image](https://github.com/TheBarret/PFFT/assets/25234371/df6a58ca-fff7-4e1f-b160-8e126d736f68)

Long run ICMP Spectrum (Samplerate: 512):
![image](https://github.com/TheBarret/PFFT/assets/25234371/299bf54e-89ed-4290-b88e-d2d9cdacb8b9)
