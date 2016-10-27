using System;

public class SupersonicPlacement
{
    private string placementName;
    private int rewardAmount;
    private string rewardName;

    public SupersonicPlacement(string pName, string rName, int rAmount)
    {
        this.placementName = pName;
        this.rewardName = rName;
        this.rewardAmount = rAmount;
    }

    public string getPlacementName()
    {
        return this.placementName;
    }

    public int getRewardAmount()
    {
        return this.rewardAmount;
    }

    public string getRewardName()
    {
        return this.rewardName;
    }

    public override string ToString()
    {
        object[] objArray1 = new object[] { this.placementName, " : ", this.rewardName, " : ", this.rewardAmount };
        return string.Concat(objArray1);
    }
}

