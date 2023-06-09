# Simple Prevent File Restore
A simple file restore prevention tool that permanently securely wipes deleted files using cryptographically random data, preventing file recovery tools from finding anything.

Windows is the only supported platform currently. Other OS's can be added easily.

This tool does not delete/modify any existing files.

<img width="635" alt="image" src="https://github.com/jamieyello/Simple-Prevent-File-Restore/assets/10054829/b24db2a6-ac8c-4f45-80c9-ab7b4c18bf86">

## Why?
When an OS deletes a file, it's typically not deleted fully. It's space is just marked as available to use, and any data will remain there until another file overwrites it. This can lead to data theft. For example, say you discard your old hard drive you no longer want. If thrown into the trash or recycled, it will very likely be picked out, where anyone down the line will have full access to anything that was on that drive.

## What does this do?
This small program simply fills a drive with one big file on a specified drive containing cryptographically random data, and then deletes it. This prevents any (once deleted) data from being recovered using any conventional file recovery tool. Doing multiple passes will ensure any data is completely beyond recovery by even the most capable actors.

The code is made as simple as possible for better transparency. see [PreventRestoreJob.cs](https://github.com/jamieyello/Simple-Prevent-File-Restore/blob/master/PreventRestoreJob.cs) to see exactly what is done.

## How should I use it?
While a hard drive has virtually unlimited read/write cycles, an SSD only has about 100,000 in total. Keep this in mind regarding how often you use this tool.

## How do I use it?
Run the exe without any arguments and follow the prompts, or use the `--help` command to see your options.
