# BFNet
[![buddy pipeline](https://app.buddy.works/cainy-a/bfnet/pipelines/pipeline/313407/badge.svg?token=537b401f436f16f2fb7cc5edefd041f3aa593895b89e70cf2ca999b54ea0fdb8 "buddy pipeline")](https://app.buddy.works/cainy-a/bfnet/pipelines/pipeline/313407)

An optimising Brainf*ck interpreter with (almost) unlimited 16-bit memory cells, written in C#, with a nod to my failed previous attempt [here](https://github.com/cainy-a/MindMangler).

16 bits should be the most you'll need.
I considered using 64-bits to make it faster for CPUs to deal with, but decided that it was wasteful.

The maximum amount of memory cells is `2,147,483,647` (the signed 32-bit limit). I could have used `BigInteger` for unlimited cells, but that is slow. I was going to use a `ulong` (unsigned 64 bit integer), but indexing in dotnet uses `int` (signed 32 bit integer) for everything.
