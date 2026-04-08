using System;
using System.Text;

namespace hashes;

public class GhostsTask : 
	IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
	IMagic
{
    private Vector vector;
    private Segment segment;
    private Cat cat;
    private Document document;
    private byte[] documentBytes;
    private Robot robot;


    // Чтобы класс одновременно реализовывал интерфейсы IFactory<A> и IFactory<B> 
    // придется воспользоваться так называемой явной реализацией интерфейса.
    // Чтобы отличать методы создания A и B у каждого метода Create нужно явно указать, к какому интерфейсу он относится.
    // На самом деле такое вы уже видели, когда реализовывали IEnumerable<T>.
    
	public void DoMagic()
	{
		DoMagicVector();
		DoMagicSegment();
		DoMagicCat();
		DoMagicDocument();
		DoMagicRobot();
	}

	private void DoMagicVector()
	{
		if (vector != null)
		{
			vector.Add(new Vector(1, 1));
		}
	}

	private void DoMagicSegment()
	{
		if (segment != null)
		{
			segment.Start.Add(new Vector(1, 1));
		}
	}

	private void DoMagicCat()
	{
		if (cat != null)
		{
			cat.Rename("NewName");
		}
	}

	private void DoMagicDocument()
	{
		if (documentBytes != null)
		{
			documentBytes[0]++;
		}
	}

	private void DoMagicRobot()
	{
		if (robot != null)
		{
			Robot.BatteryCapacity++;
		}
	}

	Vector IFactory<Vector>.Create()
	{
        if (vector == null) vector = new Vector(0, 0);
		return vector;
	}

	Segment IFactory<Segment>.Create()
	{
        if (segment == null) segment = new Segment(new Vector(0, 0), new Vector(1, 1));
		return segment;
	}

    Document IFactory<Document>.Create()
	{
        if (document == null)
        {
            documentBytes = new byte[] { 1, 2, 3 };
            document = new Document("Title", Encoding.UTF8, documentBytes);
        }
		return document;
	}

    Cat IFactory<Cat>.Create()
	{
        if (cat == null) cat = new Cat("Name", "Breed", DateTime.Now);
		return cat;
	}

    Robot IFactory<Robot>.Create()
	{
        if (robot == null) robot = new Robot("id");
		return robot;
	}
}
