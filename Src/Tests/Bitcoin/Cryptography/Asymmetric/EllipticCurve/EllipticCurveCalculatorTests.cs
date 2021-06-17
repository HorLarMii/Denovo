﻿// Autarkysoft Tests
// Copyright (c) 2020 Autarkysoft
// Distributed under the MIT software license, see the accompanying
// file LICENCE or http://www.opensource.org/licenses/mit-license.php.

using Autarkysoft.Bitcoin.Cryptography.Asymmetric.EllipticCurve;
using Autarkysoft.Bitcoin.Cryptography.Asymmetric.KeyPairs;
using System.Collections.Generic;
using Xunit;

namespace Tests.Bitcoin.Cryptography.Asymmetric.EllipticCurve
{
    public class EllipticCurveCalculatorTests
    {
        private readonly EllipticCurveCalculator calc = new();


        // The following tests are from https://github.com/bitcoin/bips/blob/master/bip-0340/test-vectors.csv
        public static IEnumerable<object[]> GetSchnorrCases()
        {
            yield return new object[]
            {
                Helper.HexToBytes("0000000000000000000000000000000000000000000000000000000000000003"), // Key
                Helper.HexToBytes("0000000000000000000000000000000000000000000000000000000000000000"), // AuxRand
                Helper.HexToBytes("0000000000000000000000000000000000000000000000000000000000000000"), // Message
                new Signature(
                    Helper.HexToBigInt("E907831F80848D1069A5371B402410364BDF1C5F8307B0084C55F1CE2DCA8215"),
                    Helper.HexToBigInt("25F66A4A85EA8B71E482A74F382D2CE5EBEEE8FDB2172F477DF4900D310536C0"),
                    SigHashType.Default)

            };
            yield return new object[]
            {
                Helper.HexToBytes("B7E151628AED2A6ABF7158809CF4F3C762E7160F38B4DA56A784D9045190CFEF"), // Key
                Helper.HexToBytes("0000000000000000000000000000000000000000000000000000000000000001"), // AuxRand
                Helper.HexToBytes("243F6A8885A308D313198A2E03707344A4093822299F31D0082EFA98EC4E6C89"), // Message
                new Signature(
                    Helper.HexToBigInt("6896BD60EEAE296DB48A229FF71DFE071BDE413E6D43F917DC8DCF8C78DE3341"),
                    Helper.HexToBigInt("8906D11AC976ABCCB20B091292BFF4EA897EFCB639EA871CFA95F6DE339E4B0A"),
                    SigHashType.Default)

            };
            yield return new object[]
            {
                Helper.HexToBytes("C90FDAA22168C234C4C6628B80DC1CD129024E088A67CC74020BBEA63B14E5C9"), // Key
                Helper.HexToBytes("C87AA53824B4D7AE2EB035A2B5BBBCCC080E76CDC6D1692C4B0B62D798E6D906"), // AuxRand
                Helper.HexToBytes("7E2D58D8B3BCDF1ABADEC7829054F90DDA9805AAB56C77333024B9D0A508B75C"), // Message
                new Signature(
                    Helper.HexToBigInt("5831AAEED7B44BB74E5EAB94BA9D4294C49BCF2A60728D8B4C200F50DD313C1B"),
                    Helper.HexToBigInt("AB745879A5AD954A72C45A91C3A51D3C7ADEA98D82F8481E0E1E03674A6F3FB7"),
                    SigHashType.Default)

            };
            yield return new object[]
            {
                Helper.HexToBytes("0B432B2677937381AEF05BB02A66ECD012773062CF3FA2549E44F58ED2401710"), // Key
                Helper.HexToBytes("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"), // AuxRand
                Helper.HexToBytes("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"), // Message
                new Signature(
                    Helper.HexToBigInt("7EB0509757E246F19449885651611CB965ECC1A187DD51B64FDA1EDC9637D5EC"),
                    Helper.HexToBigInt("97582B9CB13DB3933705B32BA982AF5AF25FD78881EBB32771FC5922EFC66EA3"),
                    SigHashType.Default)

            };
        }
        [Theory]
        [MemberData(nameof(GetSchnorrCases))]
        public void SignSchnorrTest(byte[] key, byte[] aux, byte[] hash, Signature expectedSig)
        {
            var rng = new MockRng(aux);
            Signature actualSig = calc.SignSchnorr(hash, key, rng);

            Assert.Equal(expectedSig.R, actualSig.R);
            Assert.Equal(expectedSig.S, actualSig.S);
        }

        public static IEnumerable<object[]> GetSchnorrVerifyCases()
        {
            yield return new object[]
            {
                "F9308A019258C31049344F85F89D5229B531C845836F99B08601F113BCE036F9",
                "0000000000000000000000000000000000000000000000000000000000000000",
                "E907831F80848D1069A5371B402410364BDF1C5F8307B0084C55F1CE2DCA821525F66A4A85EA8B71E482A74F382D2CE5EBEEE8FDB2172F477DF4900D310536C0"
            };
            yield return new object[]
            {
                "DFF1D77F2A671C5F36183726DB2341BE58FEAE1DA2DECED843240F7B502BA659",
                "243F6A8885A308D313198A2E03707344A4093822299F31D0082EFA98EC4E6C89",
                "6896BD60EEAE296DB48A229FF71DFE071BDE413E6D43F917DC8DCF8C78DE33418906D11AC976ABCCB20B091292BFF4EA897EFCB639EA871CFA95F6DE339E4B0A"
            };
            yield return new object[]
            {
                "DD308AFEC5777E13121FA72B9CC1B7CC0139715309B086C960E18FD969774EB8",
                "7E2D58D8B3BCDF1ABADEC7829054F90DDA9805AAB56C77333024B9D0A508B75C",
                "5831AAEED7B44BB74E5EAB94BA9D4294C49BCF2A60728D8B4C200F50DD313C1BAB745879A5AD954A72C45A91C3A51D3C7ADEA98D82F8481E0E1E03674A6F3FB7"
            };
            yield return new object[]
            {
                "25D1DFF95105F5253C4022F628A996AD3A0D95FBF21D468A1B33F8C160D8F517",
                "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                "7EB0509757E246F19449885651611CB965ECC1A187DD51B64FDA1EDC9637D5EC97582B9CB13DB3933705B32BA982AF5AF25FD78881EBB32771FC5922EFC66EA3"
            };
            yield return new object[]
            {
                "D69C3509BB99E412E68B0FE8544E72837DFA30746D8BE2AA65975F29D22DC7B9",
                "4DF3C3F68FCC83B27E9D42C90431A72499F17875C81A599B566C9889B9696703",
                "00000000000000000000003B78CE563F89A0ED9414F5AA28AD0D96D6795F9C6376AFB1548AF603B3EB45C9F8207DEE1060CB71C04E80F593060B07D28308D7F4"
            };
        }
        [Theory]
        [MemberData(nameof(GetSchnorrVerifyCases))]
        public void VerifySchnorrTest(string pubHex, string msgHex, string sigHex)
        {
            byte[] hash = Helper.HexToBytes(msgHex);
            Assert.True(Signature.TryReadSchnorr(Helper.HexToBytes(sigHex), out Signature sig, out string err), err);
            Assert.True(PublicKey.TryReadTaproot(Helper.HexToBytes(pubHex), out PublicKey pubKey) == PublicKey.PublicKeyType.Schnorr);

            Assert.Equal(Helper.HexToBytes(sigHex), sig.ToByteArraySchnorr());

            bool b = calc.VerifySchnorr(hash, sig, pubKey);
            Assert.True(b);
        }


        public static IEnumerable<object[]> GetSchnorrVerifyFailCases()
        {
            byte[] hash = Helper.HexToBytes("243F6A8885A308D313198A2E03707344A4093822299F31D0082EFA98EC4E6C89");
            Assert.True(PublicKey.TryReadTaproot(Helper.HexToBytes("DFF1D77F2A671C5F36183726DB2341BE58FEAE1DA2DECED843240F7B502BA659"), out PublicKey pubKey) == PublicKey.PublicKeyType.Schnorr);

            yield return new object[]
            {
                pubKey, hash,
                "FFF97BD5755EEEA420453A14355235D382F6472F8568A18B2F057A14602975563CC27944640AC607CD107AE10923D9EF7A73C643E16" +
                "6BE5EBEAFA34B1AC553E2"
            };
            yield return new object[]
            {
                pubKey, hash,
                "1FA62E331EDBC21C394792D2AB1100A7B432B013DF3F6FF4F99FCB33E0E1515F28890B3EDB6E7189B630448B515CE4F8622A954CFE5" +
                "45735AAEA5134FCCDB2BD"
            };
            yield return new object[]
            {
                pubKey, hash,
                "6CFF5C3BA86C69EA4B7376F31A9BCB4F74C1976089B2D9963DA2E5543E177769961764B3AA9B2FFCB6EF947B6887A226E8D7C93E00C" +
                "5ED0C1834FF0D0C2E6DA6"
            };
            yield return new object[]
            {
                pubKey, hash,
                "0000000000000000000000000000000000000000000000000000000000000000123DDA8328AF9C23A94C1FEECFD123BA4FB73476F0D" +
                "594DCB65C6425BD186051"
            };
            yield return new object[]
            {
                pubKey, hash,
                "00000000000000000000000000000000000000000000000000000000000000017615FBAF5AE28864013C099742DEADB4DBA87F11AC6" +
                "754F93780D5A1837CF197"
            };
            yield return new object[]
            {
                pubKey, hash,
                "4A298DACAE57395A15D0795DDBFD1DCB564DA82B0F269BC70A74F8220429BA1D69E89B4C5564D00349106B8497785DD7D1D713A8AE8" +
                "2B32FA79D5F7FC407D39B"
            };
            yield return new object[]
            {
                pubKey, hash,
                "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F69E89B4C5564D00349106B8497785DD7D1D713A8AE82" +
                "B32FA79D5F7FC407D39B"
            };
            yield return new object[]
            {
                pubKey, hash,
                "6CFF5C3BA86C69EA4B7376F31A9BCB4F74C1976089B2D9963DA2E5543E177769FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF4" +
                "8A03BBFD25E8CD0364141"
            };
        }
        [Theory]
        [MemberData(nameof(GetSchnorrVerifyFailCases))]
        public void VerifySchnorr_FailTest(PublicKey pubKey, byte[] hash, string sigHex)
        {
            Assert.True(Signature.TryReadSchnorr(Helper.HexToBytes(sigHex), out Signature sig, out string err), err);
            Assert.Equal(Helper.HexToBytes(sigHex), sig.ToByteArraySchnorr());

            bool b = calc.VerifySchnorr(hash, sig, pubKey);
            Assert.False(b);
        }
    }
}
