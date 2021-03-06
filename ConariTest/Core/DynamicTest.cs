﻿using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.r_eg.Conari;
using net.r_eg.Conari.Core;

namespace net.r_eg.ConariTest.Core
{
    [TestClass]
    public class DynamicTest
    {
        [TestMethod]
        public void CreateEmptyTypeTest1()
        {
            var expRetType = typeof(void);

            Type type   = Dynamic.CreateEmptyType(expRetType);
            var mi      = type.GetMethod(Dynamic.METHOD_NAME);

            Assert.AreNotEqual(null, mi);
            Assert.AreEqual(expRetType, mi.ReturnType);
            Assert.AreEqual(0, mi.GetParameters().Length);
        }

        [TestMethod]
        public void CreateEmptyTypeTest2()
        {
            var expRetType  = typeof(bool);
            var expArg0Type = typeof(Int64);
            var expArg1Type = typeof(bool);
            var expArg2Type = typeof(sbyte);

            Type type   = Dynamic.CreateEmptyType("MyFunc", expRetType, expArg0Type, expArg1Type, expArg2Type);
            var mi      = type.GetMethod("MyFunc");

            Assert.AreEqual(3, mi.GetParameters().Length);
            Assert.AreEqual(expArg0Type, mi.GetParameters()[0].ParameterType);
            Assert.AreEqual(expArg1Type, mi.GetParameters()[1].ParameterType);
            Assert.AreEqual(expArg2Type, mi.GetParameters()[2].ParameterType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateEmptyTypeTest3()
        {
            Dynamic.CreateEmptyType((string)null, typeof(void));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateEmptyTypeTest4()
        {
            Dynamic.CreateEmptyType(" ", typeof(void));
        }

        [TestMethod]
        public void GetMethodInfoTest1()
        {
            var expRetType = typeof(void);

            MethodInfo mi = Dynamic.GetMethodInfo(false, expRetType);

            Assert.AreNotEqual(null, mi);
            Assert.AreEqual(Dynamic.METHOD_NAME, mi.Name);
            Assert.AreEqual(expRetType, mi.ReturnType);
            Assert.AreEqual(0, mi.GetParameters().Length);
        }

        [TestMethod]
        public void GetMethodInfoTest2()
        {
            var expRetType  = typeof(IntPtr);
            var expArg0Type = typeof(int);
            var expArg1Type = typeof(bool);

            MethodInfo mi = Dynamic.GetMethodInfo("MyFunc", expRetType, expArg0Type, expArg1Type);

            Assert.AreEqual(2, mi.GetParameters().Length);
            Assert.AreEqual("MyFunc", mi.Name);
            Assert.AreEqual(expArg0Type, mi.GetParameters()[0].ParameterType);
            Assert.AreEqual(expArg1Type, mi.GetParameters()[1].ParameterType);
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentException))] - this behaviour has been changed.
        public void GetMethodInfoTest3()
        {
            Assert.AreEqual(Dynamic.METHOD_NAME, Dynamic.GetMethodInfo((string)null, false, typeof(void)).Name);
            Assert.AreEqual(Dynamic.METHOD_NAME, Dynamic.GetMethodInfo(" ", false, typeof(void)).Name);
        }

        [TestMethod]
        public void CastTest1()
        {
            Assert.AreEqual(0x3F, Dynamic.Cast<byte>(0x3F));
            Assert.AreEqual(3, Dynamic.DCast(typeof(int), 3.14f));

            Assert.AreEqual(typeof(IntPtr), Dynamic.DCast(typeof(IntPtr), 17).GetType());
            Assert.AreEqual(typeof(char), Dynamic.DCast(typeof(char), (byte)0x3F).GetType());

            Assert.AreEqual(null, Dynamic.DCast(typeof(void), 17));
        }

        [TestMethod]
        public void cacheTest1()
        {
            using(var l = new ConariL(
                                 new Config("") {
                                     LazyLoading = true
                                 }))
            {
                Dynamic._.UseCache = true;

                Assert.AreEqual("m1", Dynamic.GetMethodInfo("m1", false, typeof(bool), typeof(int)).Name);
                Assert.AreEqual("m2", Dynamic.GetMethodInfo("m2", typeof(bool), typeof(int)).Name);
                Assert.AreEqual("m2", Dynamic.GetMethodInfo("m3", typeof(bool), typeof(int)).Name);
                Assert.AreEqual("m2", Dynamic.GetMethodInfo(typeof(bool), typeof(int)).Name);
                Assert.AreEqual("m4", Dynamic.GetMethodInfo("m4", false, typeof(bool), typeof(int)).Name);
            }
        }
    }
}
