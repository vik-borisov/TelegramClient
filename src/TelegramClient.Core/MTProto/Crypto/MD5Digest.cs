using System;
using System.Text;

namespace TelegramClient.Core.MTProto.Crypto
{
    public interface IDigest
    {
        /**
* return the algorithm name
*
* @return the algorithm name
*/
        string AlgorithmName { get; }

        /**
* return the size, in bytes, of the digest produced by this message digest.
*
* @return the size, in bytes, of the digest produced by this message digest.
*/
        int GetDigestSize();

        /**
* return the size, in bytes, of the internal buffer used by this digest.
*
* @return the size, in bytes, of the internal buffer used by this digest.
*/
        int GetByteLength();

        /**
* update the message digest with a single byte.
*
* @param inByte the input byte to be entered.
*/
        void Update(byte input);

        /**
* update the message digest with a block of bytes.
*
* @param input the byte array containing the data.
* @param inOff the offset into the byte array where the data starts.
* @param len the length of the data.
*/
        void BlockUpdate(byte[] input, int inOff, int length);

        /**
* Close the digest, producing the final digest value. The doFinal
* call leaves the digest reset.
*
* @param output the array the digest is to be copied into.
* @param outOff the offset into the out array the digest is to start at.
*/
        int DoFinal(byte[] output, int outOff);

        /**
* reset the digest back to it's initial state.
*/
        void Reset();
    }

    public class Md5
    {
        private readonly Md5Digest _digest = new Md5Digest();

        public static string GetMd5String(string data)
        {
            return BitConverter.ToString(GetMd5Bytes(Encoding.UTF8.GetBytes(data))).Replace("-", "").ToLower();
        }

        public static byte[] GetMd5Bytes(byte[] data)
        {
            var digest = new Md5Digest();
            digest.BlockUpdate(data, 0, data.Length);
            var hash = new byte[16];
            digest.DoFinal(hash, 0);

            return hash;
        }

        public void Update(byte[] chunk)
        {
            _digest.BlockUpdate(chunk, 0, chunk.Length);
        }

        public void Update(byte[] chunk, int offset, int limit)
        {
            _digest.BlockUpdate(chunk, offset, limit);
        }

        public string FinalString()
        {
            var hash = new byte[16];
            _digest.DoFinal(hash, 0);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    public abstract class GeneralDigest
        : IDigest
    {
        private const int ByteLength = 64;

        private readonly byte[] _xBuf;

        private long _byteCount;
        private int _xBufOff;

        internal GeneralDigest()
        {
            _xBuf = new byte[4];
        }

        internal GeneralDigest(GeneralDigest t)
        {
            _xBuf = new byte[t._xBuf.Length];
            Array.Copy(t._xBuf, 0, _xBuf, 0, t._xBuf.Length);

            _xBufOff = t._xBufOff;
            _byteCount = t._byteCount;
        }

        public void Update(byte input)
        {
            _xBuf[_xBufOff++] = input;

            if (_xBufOff == _xBuf.Length)
            {
                ProcessWord(_xBuf, 0);
                _xBufOff = 0;
            }

            _byteCount++;
        }

        public void BlockUpdate(
            byte[] input,
            int inOff,
            int length)
        {
            //
            // fill the current word
            //
            while (_xBufOff != 0 && length > 0)
            {
                Update(input[inOff]);
                inOff++;
                length--;
            }

            //
            // process whole words.
            //
            while (length > _xBuf.Length)
            {
                ProcessWord(input, inOff);

                inOff += _xBuf.Length;
                length -= _xBuf.Length;
                _byteCount += _xBuf.Length;
            }

            //
            // load in the remainder.
            //
            while (length > 0)
            {
                Update(input[inOff]);

                inOff++;
                length--;
            }
        }

        public virtual void Reset()
        {
            _byteCount = 0;
            _xBufOff = 0;
            Array.Clear(_xBuf, 0, _xBuf.Length);
        }

        public int GetByteLength()
        {
            return ByteLength;
        }

        public abstract string AlgorithmName { get; }
        public abstract int GetDigestSize();
        public abstract int DoFinal(byte[] output, int outOff);

        public void Finish()
        {
            var bitLength = _byteCount << 3;

            //
            // add the pad bytes.
            //
            Update(128);

            while (_xBufOff != 0) Update(0);
            ProcessLength(bitLength);
            ProcessBlock();
        }

        internal abstract void ProcessWord(byte[] input, int inOff);
        internal abstract void ProcessLength(long bitLength);
        internal abstract void ProcessBlock();
    }

    public class Md5Digest
        : GeneralDigest
    {
        private const int DigestLength = 16;

        //
        // round 1 left rotates
        //
        private static readonly int S11 = 7;

        private static readonly int S12 = 12;
        private static readonly int S13 = 17;
        private static readonly int S14 = 22;

        //
        // round 2 left rotates
        //
        private static readonly int S21 = 5;

        private static readonly int S22 = 9;
        private static readonly int S23 = 14;
        private static readonly int S24 = 20;

        //
        // round 3 left rotates
        //
        private static readonly int S31 = 4;

        private static readonly int S32 = 11;
        private static readonly int S33 = 16;
        private static readonly int S34 = 23;

        //
        // round 4 left rotates
        //
        private static readonly int S41 = 6;

        private static readonly int S42 = 10;
        private static readonly int S43 = 15;
        private static readonly int S44 = 21;
        private readonly int[] _x = new int[16];
        private int _h1, _h2, _h3, _h4; // IV's
        private int _xOff;

        public Md5Digest()
        {
            Reset();
        }

        /**
* Copy constructor. This will copy the state of the provided
* message digest.
*/

        public Md5Digest(Md5Digest t)
            : base(t)
        {
            _h1 = t._h1;
            _h2 = t._h2;
            _h3 = t._h3;
            _h4 = t._h4;

            Array.Copy(t._x, 0, _x, 0, t._x.Length);
            _xOff = t._xOff;
        }

        public override string AlgorithmName => "MD5";

        public override int GetDigestSize()
        {
            return DigestLength;
        }

        internal override void ProcessWord(
            byte[] input,
            int inOff)
        {
            _x[_xOff++] = (input[inOff] & 0xff) | ((input[inOff + 1] & 0xff) << 8)
                        | ((input[inOff + 2] & 0xff) << 16) | ((input[inOff + 3] & 0xff) << 24);

            if (_xOff == 16)
                ProcessBlock();
        }

        internal override void ProcessLength(
            long bitLength)
        {
            if (_xOff > 14)
                ProcessBlock();

            _x[14] = (int) (bitLength & 0xffffffff);
            _x[15] = (int) ((ulong) bitLength >> 32);
        }

        private void UnpackWord(
            int word,
            byte[] outBytes,
            int outOff)
        {
            outBytes[outOff] = (byte) word;
            outBytes[outOff + 1] = (byte) ((uint) word >> 8);
            outBytes[outOff + 2] = (byte) ((uint) word >> 16);
            outBytes[outOff + 3] = (byte) ((uint) word >> 24);
        }

        public override int DoFinal(
            byte[] output,
            int outOff)
        {
            Finish();

            UnpackWord(_h1, output, outOff);
            UnpackWord(_h2, output, outOff + 4);
            UnpackWord(_h3, output, outOff + 8);
            UnpackWord(_h4, output, outOff + 12);

            Reset();

            return DigestLength;
        }

        /**
* reset the chaining variables to the IV values.
*/

        public override void Reset()
        {
            base.Reset();

            _h1 = 0x67452301;
            _h2 = unchecked((int) 0xefcdab89);
            _h3 = unchecked((int) 0x98badcfe);
            _h4 = 0x10325476;

            _xOff = 0;

            for (var i = 0; i != _x.Length; i++)
                _x[i] = 0;
        }

        /*
* rotate int x left n bits.
*/

        private int RotateLeft(
            int x,
            int n)
        {
            return (x << n) | (int) ((uint) x >> (32 - n));
        }

        /*
* F, G, H and I are the basic MD5 functions.
*/

        private int F(
            int u,
            int v,
            int w)
        {
            return (u & v) | (~u & w);
        }

        private int G(
            int u,
            int v,
            int w)
        {
            return (u & w) | (v & ~w);
        }

        private int H(
            int u,
            int v,
            int w)
        {
            return u ^ v ^ w;
        }

        private int K(
            int u,
            int v,
            int w)
        {
            return v ^ (u | ~w);
        }

        internal override void ProcessBlock()
        {
            var a = _h1;
            var b = _h2;
            var c = _h3;
            var d = _h4;

            //
            // Round 1 - F cycle, 16 times.
            //
            a = RotateLeft(a + F(b, c, d) + _x[0] + unchecked((int) 0xd76aa478), S11) + b;
            d = RotateLeft(d + F(a, b, c) + _x[1] + unchecked((int) 0xe8c7b756), S12) + a;
            c = RotateLeft(c + F(d, a, b) + _x[2] + 0x242070db, S13) + d;
            b = RotateLeft(b + F(c, d, a) + _x[3] + unchecked((int) 0xc1bdceee), S14) + c;
            a = RotateLeft(a + F(b, c, d) + _x[4] + unchecked((int) 0xf57c0faf), S11) + b;
            d = RotateLeft(d + F(a, b, c) + _x[5] + 0x4787c62a, S12) + a;
            c = RotateLeft(c + F(d, a, b) + _x[6] + unchecked((int) 0xa8304613), S13) + d;
            b = RotateLeft(b + F(c, d, a) + _x[7] + unchecked((int) 0xfd469501), S14) + c;
            a = RotateLeft(a + F(b, c, d) + _x[8] + 0x698098d8, S11) + b;
            d = RotateLeft(d + F(a, b, c) + _x[9] + unchecked((int) 0x8b44f7af), S12) + a;
            c = RotateLeft(c + F(d, a, b) + _x[10] + unchecked((int) 0xffff5bb1), S13) + d;
            b = RotateLeft(b + F(c, d, a) + _x[11] + unchecked((int) 0x895cd7be), S14) + c;
            a = RotateLeft(a + F(b, c, d) + _x[12] + 0x6b901122, S11) + b;
            d = RotateLeft(d + F(a, b, c) + _x[13] + unchecked((int) 0xfd987193), S12) + a;
            c = RotateLeft(c + F(d, a, b) + _x[14] + unchecked((int) 0xa679438e), S13) + d;
            b = RotateLeft(b + F(c, d, a) + _x[15] + 0x49b40821, S14) + c;

            //
            // Round 2 - G cycle, 16 times.
            //
            a = RotateLeft(a + G(b, c, d) + _x[1] + unchecked((int) 0xf61e2562), S21) + b;
            d = RotateLeft(d + G(a, b, c) + _x[6] + unchecked((int) 0xc040b340), S22) + a;
            c = RotateLeft(c + G(d, a, b) + _x[11] + 0x265e5a51, S23) + d;
            b = RotateLeft(b + G(c, d, a) + _x[0] + unchecked((int) 0xe9b6c7aa), S24) + c;
            a = RotateLeft(a + G(b, c, d) + _x[5] + unchecked((int) 0xd62f105d), S21) + b;
            d = RotateLeft(d + G(a, b, c) + _x[10] + 0x02441453, S22) + a;
            c = RotateLeft(c + G(d, a, b) + _x[15] + unchecked((int) 0xd8a1e681), S23) + d;
            b = RotateLeft(b + G(c, d, a) + _x[4] + unchecked((int) 0xe7d3fbc8), S24) + c;
            a = RotateLeft(a + G(b, c, d) + _x[9] + 0x21e1cde6, S21) + b;
            d = RotateLeft(d + G(a, b, c) + _x[14] + unchecked((int) 0xc33707d6), S22) + a;
            c = RotateLeft(c + G(d, a, b) + _x[3] + unchecked((int) 0xf4d50d87), S23) + d;
            b = RotateLeft(b + G(c, d, a) + _x[8] + 0x455a14ed, S24) + c;
            a = RotateLeft(a + G(b, c, d) + _x[13] + unchecked((int) 0xa9e3e905), S21) + b;
            d = RotateLeft(d + G(a, b, c) + _x[2] + unchecked((int) 0xfcefa3f8), S22) + a;
            c = RotateLeft(c + G(d, a, b) + _x[7] + 0x676f02d9, S23) + d;
            b = RotateLeft(b + G(c, d, a) + _x[12] + unchecked((int) 0x8d2a4c8a), S24) + c;

            //
            // Round 3 - H cycle, 16 times.
            //
            a = RotateLeft(a + H(b, c, d) + _x[5] + unchecked((int) 0xfffa3942), S31) + b;
            d = RotateLeft(d + H(a, b, c) + _x[8] + unchecked((int) 0x8771f681), S32) + a;
            c = RotateLeft(c + H(d, a, b) + _x[11] + 0x6d9d6122, S33) + d;
            b = RotateLeft(b + H(c, d, a) + _x[14] + unchecked((int) 0xfde5380c), S34) + c;
            a = RotateLeft(a + H(b, c, d) + _x[1] + unchecked((int) 0xa4beea44), S31) + b;
            d = RotateLeft(d + H(a, b, c) + _x[4] + 0x4bdecfa9, S32) + a;
            c = RotateLeft(c + H(d, a, b) + _x[7] + unchecked((int) 0xf6bb4b60), S33) + d;
            b = RotateLeft(b + H(c, d, a) + _x[10] + unchecked((int) 0xbebfbc70), S34) + c;
            a = RotateLeft(a + H(b, c, d) + _x[13] + 0x289b7ec6, S31) + b;
            d = RotateLeft(d + H(a, b, c) + _x[0] + unchecked((int) 0xeaa127fa), S32) + a;
            c = RotateLeft(c + H(d, a, b) + _x[3] + unchecked((int) 0xd4ef3085), S33) + d;
            b = RotateLeft(b + H(c, d, a) + _x[6] + 0x04881d05, S34) + c;
            a = RotateLeft(a + H(b, c, d) + _x[9] + unchecked((int) 0xd9d4d039), S31) + b;
            d = RotateLeft(d + H(a, b, c) + _x[12] + unchecked((int) 0xe6db99e5), S32) + a;
            c = RotateLeft(c + H(d, a, b) + _x[15] + 0x1fa27cf8, S33) + d;
            b = RotateLeft(b + H(c, d, a) + _x[2] + unchecked((int) 0xc4ac5665), S34) + c;

            //
            // Round 4 - K cycle, 16 times.
            //
            a = RotateLeft(a + K(b, c, d) + _x[0] + unchecked((int) 0xf4292244), S41) + b;
            d = RotateLeft(d + K(a, b, c) + _x[7] + 0x432aff97, S42) + a;
            c = RotateLeft(c + K(d, a, b) + _x[14] + unchecked((int) 0xab9423a7), S43) + d;
            b = RotateLeft(b + K(c, d, a) + _x[5] + unchecked((int) 0xfc93a039), S44) + c;
            a = RotateLeft(a + K(b, c, d) + _x[12] + 0x655b59c3, S41) + b;
            d = RotateLeft(d + K(a, b, c) + _x[3] + unchecked((int) 0x8f0ccc92), S42) + a;
            c = RotateLeft(c + K(d, a, b) + _x[10] + unchecked((int) 0xffeff47d), S43) + d;
            b = RotateLeft(b + K(c, d, a) + _x[1] + unchecked((int) 0x85845dd1), S44) + c;
            a = RotateLeft(a + K(b, c, d) + _x[8] + 0x6fa87e4f, S41) + b;
            d = RotateLeft(d + K(a, b, c) + _x[15] + unchecked((int) 0xfe2ce6e0), S42) + a;
            c = RotateLeft(c + K(d, a, b) + _x[6] + unchecked((int) 0xa3014314), S43) + d;
            b = RotateLeft(b + K(c, d, a) + _x[13] + 0x4e0811a1, S44) + c;
            a = RotateLeft(a + K(b, c, d) + _x[4] + unchecked((int) 0xf7537e82), S41) + b;
            d = RotateLeft(d + K(a, b, c) + _x[11] + unchecked((int) 0xbd3af235), S42) + a;
            c = RotateLeft(c + K(d, a, b) + _x[2] + 0x2ad7d2bb, S43) + d;
            b = RotateLeft(b + K(c, d, a) + _x[9] + unchecked((int) 0xeb86d391), S44) + c;

            _h1 += a;
            _h2 += b;
            _h3 += c;
            _h4 += d;

            //
            // reset the offset and clean out the word buffer.
            //
            _xOff = 0;
            for (var i = 0; i != _x.Length; i++)
                _x[i] = 0;
        }
    }
}