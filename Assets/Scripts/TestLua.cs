using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class TestLua : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaEnv lua = new LuaEnv();
        lua.AddLoader((ref string filename) =>
        {
            if (filename == "LuaTest")
            {
                //string script = Application.dataPath + "/LuaScripts/" + filename + ".lua";
                string script = "G:/gongcheng/" + filename + ".lua";
                string strLuaContent = File.ReadAllText(script);

                return System.Text.Encoding.UTF8.GetBytes(strLuaContent);
            }
            return null;
        });
        lua.DoString("require 'LuaTest'");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
