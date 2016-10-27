using System;

public class SupersonicError
{
    private int code;
    private string description;

    public SupersonicError(int errCode, string errDescription)
    {
        this.code = errCode;
        this.description = errDescription;
    }

    public int getCode()
    {
        return this.code;
    }

    public string getDescription()
    {
        return this.description;
    }

    public int getErrorCode()
    {
        return this.code;
    }

    public override string ToString()
    {
        return (this.code + " : " + this.description);
    }
}

