# PFFT
Showing ICMP TTL data in a Fast Fourier Transformer (POC/WIP)

FFT of ICMP latency:

- Identifying periodic issues: If there are any periodic issues affecting latency, they will show up as peaks at corresponding frequencies in the FFT. For example, if there's a process that runs every 10 seconds and slows down the network, you would see a peak at the 0.1 Hz frequency.

- Identifying persistent problems: If the latency is consistently high, this will appear as a peak at the 0Hz frequency (often called the DC component in FFT results). If you see a high DC component, it could be an indication of a persistent problem with latency.

- Identifying random issues: If there are a lot of random, non-periodic issues affecting latency, these will show up as a wide, flat spectrum in the FFT. If you see a spectrum that doesn't have distinct peaks but is high across a wide range of frequencies, it could mean that there's a lot of random jitter in the latency.

Understanding the nature of latency:

By looking at the spectrum, you can get a better understanding of whether latency issues are steady, periodic, or random.
Keep in mind that interpreting the FFT results in terms of frequencies requires knowledge of the sample rate. In this context, the "sample rate" is the rate at which you're sending ICMP pings and recording the round-trip times. For example, if you're sending pings once per second, then the Nyquist frequency (the maximum frequency you can resolve) is 0.5 Hz, and the frequency of each FFT bin is given by f = bin_number / N, where N is the total number of bins (equal to the number of pings). Be sure to take this into account when interpreting your results.

Example of showing a spike in the 0,30 ~ 0,40 at a sample rate of 1024, result from www.google.nl
![image](https://github.com/TheBarret/PFFT/assets/25234371/e9e62654-9a56-42f6-96be-62a32c46bb95)

Short test run on 512 sample rate
![image](https://github.com/TheBarret/PFFT/assets/25234371/df6a58ca-fff7-4e1f-b160-8e126d736f68)

Long test run on 512 sample rate
![image](https://github.com/TheBarret/PFFT/assets/25234371/299bf54e-89ed-4290-b88e-d2d9cdacb8b9)
