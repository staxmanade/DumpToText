What is this?
--
If you've ever seen [LinqPAD](http://www.linqpad.net/) and used the `.Dump()` extension method. Then you'll get the idea behind this project.

Its goal is to try to give a quick extension method to allow you to visually trace out the state of (hopefully any) object.

Where would I use this?
--
Probably the best use case of this would be when doing some TDD. If you want to quickly trace/dump out what an object looks like in memory and have a quick visual read of its public state, then you can leverage this project to help with that.

How can I use this?
--
1. Install via NuGet `Install-Package DumpToText`
2. Now you can use the extension method `.DumpToText()`

Can you give an example of it's use?
--
Given the following sample code. The `foo.DumpToText();`...

    public class DompToTextSample
    {
        [Test]
        public void DumpIt()
        {
            var foo = new Foo();
            foo.DumpToText();
        }
    }

    public class Foo
    {
        public int SomeInteger { get { return 100; } }
        public List<string> Foos
        {
            get { return new List<string> { "a", "b", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut a tortor vitae eros hendrerit imperdiet vitae vel neque." }; }
        }
    }

...will output the below view (in my case the ReSharper test output window).
    
    |-----------------------------------------------------------------------------------------------------------------------------------------|
    | Foo                                                                                                                                     |
    |-----------------------------------------------------------------------------------------------------------------------------------------|
    | SomeInteger | 100                                                                                                                       |
    |-----------------------------------------------------------------------------------------------------------------------------------------|
    |        Foos | |----------------------------------------------------------------------------------------------------------------------|  |
    |             | | List<String> (3 items)                                                                                               |  |
    |             | |----------------------------------------------------------------------------------------------------------------------|  |
    |             | | a                                                                                                                    |  |
    |             | |----------------------------------------------------------------------------------------------------------------------|  |
    |             | | b                                                                                                                    |  |
    |             | |----------------------------------------------------------------------------------------------------------------------|  |
    |             | | Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut a tortor vitae eros hendrerit imperdiet vitae vel neque. |  |
    |             | |----------------------------------------------------------------------------------------------------------------------|  |
    |-----------------------------------------------------------------------------------------------------------------------------------------|
