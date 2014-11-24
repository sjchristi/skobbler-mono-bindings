require 'rake'

source_file = File.join('SKMaps.framework', 'SKMaps')

if not File.exists?(source_file)
	puts "Could not locate SKMaps.framework; make sure this is in the same folder as this shell script"
	exit
end

file_extension = File.extname(source_file)
file_extension = ".a" if file_extension.length == 0
source_file_basename = File.basename(source_file, ".*")
target_fixed_file = "#{source_file_basename}#{file_extension}"

utility_root = File.join('.', '__fix_lib_tmp')

puts "Making tmp working folder..."
FileUtils.rm_rf utility_root
FileUtils.mkdir_p utility_root

source_file_copy = File.join(utility_root, source_file_basename + file_extension)

puts "Copying files.."
FileUtils.cp source_file, source_file_copy

lipo_info_file = File.join(utility_root, "lipo_info.txt")
sh "lipo -info #{source_file} > #{lipo_info_file}"

lipo_info = nil
open(lipo_info_file) { |f|
    lipo_info = f.read # This returns a string even if the file is empty.
}

architectures = /Architectures in the fat file: #{source_file} are: ([A-Za-z0-9_ ]+)/.match(lipo_info)[1].strip.split(' ')

puts "Found the following architectures:"
architectures.each { |a| puts "\t#{a}" }

arch_files = architectures.map { |a| "#{source_file_basename}-#{a}-new.a"}

architectures.each do |arch|
	puts "Processing #{arch}..."
	root_path = File.join(utility_root, arch)
	FileUtils.mkdir_p(root_path)
	puts "  Splitting library..."
	sh "    cd #{utility_root} && lipo -thin #{arch} -output #{source_file_basename}-#{arch}.a #{source_file_basename}.a"
	sh "    cd #{root_path} && ar -x ../#{source_file_basename}-#{arch}.a"
	puts "  Removing offending symbols..."
	# remove offending files that are causing issues....
	# pngtest.o includes a definition for _main which conflicts when linking the dll into C# projects...
	FileUtils.rm_rf(File.join(root_path, 'pngtest.o'))
	puts "  Creating new library..."
	sh "    cd #{root_path} && libtool -no_warning_for_no_symbols -static -o ../#{source_file_basename}-#{arch}-new.a *.o"
end

puts "Re-creating master library..."
sh "cd #{utility_root} && lipo -create -output #{target_fixed_file} #{arch_files.join(' ')}"

puts "Copying fixed file into place..."
FileUtils.cp File.join(utility_root, target_fixed_file), File.join('.', target_fixed_file)

puts "Cleaning up..."
FileUtils.rm_rf utility_root
