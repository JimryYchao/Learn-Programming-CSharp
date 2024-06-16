.assembly extern mscorlib {auto}
.assembly HelloCIL { }
// .module HelloCIL.exe
.namespace HelloCIL
 {
    .class public Program extends [mscorlib]System.Object
    {
         .method static private void Start(string [] args)
         {
            .entrypoint
            ldstr   "Hello World!"
            call    void[mscorlib]System.Console::WriteLine(string)
	        call    string [mscorlib]System.Console::ReadLine()
	        pop
            ret
        }
    }
} 