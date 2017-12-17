# sstc
My own implementation for source code management system (SCM)

## Quick Overview

the main feature of the system is to record every change when you commit and let you checkout to this commit at any time

## Main features
1. `add`         to add files to staging area
2. `commit`      to commit changes
3. `log`         to list commits history
4. `checkout`    to change source code to this commit
5. `help`        to list manual

### initialize a repo

To initialize a repo:

```sh
init
```

It will create a directory called `.sstc` inside the current folder.<br>
Inside that directory, it will generate the initial repo structure :

```
.sstc
├── current.txt
├── master
    └── 0.txt
    └── commits_log.txt
    └── depend_on.txt
    └── tracked.txt

```
### Add files to staging area

to add files to staging area:

```sh
add <file path> ...
```

### commit changes

compute delta from origional file and current file and store change in the commit file:

```sh
commit
```
I have used flowe approach -mentioned in resources-.

### log commits

to list commits history:

```sh
log
```

### checkout

to change source code to this commit:

```sh
checkout <commit_num>
```
it reads the delta from commit then combine it with origional file then write the result in the current file
### show manual

to list manual:

```sh
help
```
## Future work

1. `status`    to list changes and new files from the last commit
2. `branch`      support branches and merge them
3.  use `Zlib library` to compress changes
4.  use `VCDIFF` to compute delta and compress it

## Resources

take your time in learning the theory and the best design you need ,this resources help me alot
The [Eric Sink's Weblog](http://www.ericsink.com/) is A good place to learn about version control , specially:

- [Time and Space Tradeoffs in Version Control Storage](http://www.ericsink.com/entries/time_space_tradeoffs.html)
- [his series of articles Source Control HOWTO](http://ericsink.com/scm/source_control.html) specially his article on Repositories
- [Available Scripts](https://github.com/facebookincubator/create-react-app/blob/master/packages/react-scripts/template/README.md#available-scripts)

Two answers on stackoverflow :
- [Writing my own file versioning program](https://stackoverflow.com/questions/902314/writing-my-own-file-versioning-program)
- [What are the core concepts of the Clearcase version control system every developer should know](https://stackoverflow.com/questions/645008/what-are-the-basic-clearcase-concepts-every-developer-should-know)


## Contributing

You can contribute by reviewing and sending feedback on code , suggesting a new features ,implement [Future work](https://github.com/ahmedakef/sstc#future-work) described above, as well as submit code fixes or code contributions of your own.