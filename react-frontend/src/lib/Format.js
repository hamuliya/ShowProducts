export function FormatDate(date) {
  const yyyy = String(date).substring(0, 4);
  const mm = String(date).substring(5, 7); //January is 0!
  const dd = String(date).substring(8, 10); //January is 0!

  const ddmmyyyy = dd + "-" + mm + "-" + yyyy;
  return ddmmyyyy;
}

export function UppcaseFirstLetter(input) {
  let output="";
  const words = input.split(" ");
  
  words.map((word) => {
    output =output+" "+word.charAt(0).toUpperCase() + word.slice(1).toLowerCase();
  });

  return output;
}


